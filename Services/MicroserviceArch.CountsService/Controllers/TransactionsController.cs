using MicroserviceArch.DAL.Entities;
using MicroserviceArch.DTOEntity;
using MicroserviceArch.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Threading.Tasks;
using MicroserviceArch.Pagination.Entities;
using System.Collections.Generic;
using MicroserviceArch.RabbitMQ;
using Microsoft.Extensions.Configuration;

namespace MicroserviceArch.CountsService.Controllers
{
    /// <summary>
    /// Контроллер транакций
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository<TransactionEntity> repository;
        private readonly ICountRepository<CountEntity> countRepository;
        private readonly IRabbitMqService rabbitMQ;
        private readonly IConfiguration configuration;
        private readonly IConfigurationSection configurationSection;

        public TransactionsController(ITransactionRepository<TransactionEntity> repository, ICountRepository<CountEntity> countRepository,
            IRabbitMqService rabbitMQ, IConfiguration configuration)
        {
            this.repository = repository;
            this.countRepository = countRepository;
            this.rabbitMQ = rabbitMQ;
            this.configuration = configuration;
            this.configurationSection = this.configuration.GetSection("RebbitMQServerHost");
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TransactionEntityDTO entityDTO)
        {
            //Создание модели сообщения
            MessageDTO messageDTO = new MessageDTO();
            messageDTO.Host = configurationSection.Value;

            //Проверка на валидность передаваемой модели
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                return BadRequest(new TransactionEntityDTO { IsSuccessful = false, Notification = $"{errors}" });
            }

            try
            {
                //Получение данных о счетах
                var senderCount = await countRepository.Get(entityDTO.CountId);
                var reciveCount = await countRepository.Get(entityDTO.CountReciverId);

                //Если баланс счета списания меньше суммы списания, то запрещаем проведение транзакции
                if (!await countRepository.CheckBalanceForTransaction(entityDTO.CountId, entityDTO.Sum))
                    return BadRequest(new TransactionEntityDTO { IsSuccessful = false, Notification = "Недостаточно средств на счету для перевода" });

                //Если счет получателя не найден
                if (reciveCount == null) 
                    return BadRequest(new TransactionEntityDTO { IsSuccessful = false, Notification = "Не найден счет получателя" });

                //Создаем модель транзакции
                TransactionEntity transactionEntity = new TransactionEntity
                {
                    Sum = entityDTO.Sum,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CountId = entityDTO.CountId,
                    CountReciverId = entityDTO.CountReciverId,
                    Description = $"Был выполнен перевод со счета {entityDTO.CountId} на счет {entityDTO.CountReciverId}"
                };

                //Производим расчеты счетов
                senderCount.Count -= entityDTO.Sum;
                reciveCount.Count += entityDTO.Sum;

                senderCount = await countRepository.Update(senderCount);
                reciveCount = await countRepository.Update(reciveCount);

                //Заносим транзакцию в базу
                var transaction = await repository.Add(transactionEntity);

                //Если транзакция не совершена, производим отмену на счетах
                if(transaction == null)
                {
                    senderCount.Count += entityDTO.Sum;
                    reciveCount.Count -= entityDTO.Sum;

                    senderCount = await countRepository.Update(senderCount);
                    reciveCount = await countRepository.Update(reciveCount);

                    return BadRequest(new TransactionEntityDTO() { IsSuccessful = false, Notification = "Возникла ошибка при переводе средств! Повторите попытку позже!" });
                }

                entityDTO.IsSuccessful= true;
                entityDTO.CreatedAt = transaction.CreatedAt;
                entityDTO.UpdatedAt = transaction.UpdatedAt;
                entityDTO.Description = transaction.Description;

                //Отправка уведомления отправителю
                messageDTO.Message = $"Списание средств. {entityDTO.Description}. Сумма: {entityDTO.Sum}. Баланс: {senderCount.Count}";
                messageDTO.Type = $"Transaction";
                messageDTO.ClientID = senderCount.ClientId;
                messageDTO.CreatedAt = transaction.CreatedAt;
                await rabbitMQ.SendMessageAsync(messageDTO);

                //Отправка уведомления получателю
                messageDTO.Message = $"Зачисление средств. {entityDTO.Description}. Сумма: {entityDTO.Sum}. Баланс: {reciveCount.Count}";
                messageDTO.Type = $"Transaction";
                messageDTO.ClientID = reciveCount.ClientId;
                messageDTO.CreatedAt = transaction.CreatedAt;
                await rabbitMQ.SendMessageAsync(messageDTO);

                return Ok(entityDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new TransactionEntityDTO { IsSuccessful = false, Notification = $"{ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PageParametrsForTransactions page)
        {
            var transactions = await repository.GetAllByCount(page.CountId);

            List<TransactionEntityDTO> entityDTOs = new List<TransactionEntityDTO>();

            if (transactions.Count == 0)
            {
                entityDTOs.Add(new TransactionEntityDTO() { IsSuccessful = true, Notification = "По данномуу счету транзакций нет" });
            }

            if(page.startDate != default && page.endDate != default)
                transactions = transactions.Where(date => page.startDate <= date.CreatedAt.DateTime && date.CreatedAt.DateTime <= page.endDate).ToList();

            switch(page.IsSortStandart)
            {
                case 0:
                    transactions = transactions.OrderBy(s => s.CreatedAt).ToList();
                    break;
                case 1:
                    transactions = transactions.OrderBy(s => s.Sum).ToList();
                    break;
                case 2:
                    transactions = transactions.OrderByDescending(s => s.Sum).ToList();
                    break;
                default:
                    transactions = transactions.OrderBy(s => s.CreatedAt).ToList();
                    break;
            }

            foreach(var transaction in transactions)
            {
                TransactionEntityDTO transactionEntityDTO = new TransactionEntityDTO()
                {
                    IsSuccessful = true,
                    CountId = transaction.CountId,
                    CountReciverId = transaction.CountReciverId,
                    Sum = transaction.Sum,
                    CreatedAt = transaction.CreatedAt,
                    Id = transaction.Id,
                    UpdatedAt = transaction.CreatedAt,
                    Description = transaction.Description,
                };

                if (transaction.CountReciverId == page.CountId)
                    transactionEntityDTO.Description = "Вам совершен перевод. " + transactionEntityDTO.Description;
                else
                    transactionEntityDTO.Description = "Вы совершили перевод. " + transactionEntityDTO.Description;

                entityDTOs.Add(transactionEntityDTO);
            }

            return Ok(entityDTOs);
        }


    }
}
