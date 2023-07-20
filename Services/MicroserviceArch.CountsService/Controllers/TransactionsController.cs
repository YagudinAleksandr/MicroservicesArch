using MicroserviceArch.DAL.Entities;
using MicroserviceArch.DTOEntity;
using MicroserviceArch.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Threading.Tasks;
using MicroserviceArch.Pagination.Entities;
using System.Collections.Generic;

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

        public TransactionsController(ITransactionRepository<TransactionEntity> repository, ICountRepository<CountEntity> countRepository)
        {
            this.repository = repository;
            this.countRepository = countRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TransactionEntityDTO entityDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                    return BadRequest(new TransactionEntityDTO { IsSuccessful = false, Notification = $"{errors}" });
                }

                if (!await countRepository.CheckBalanceForTransaction(entityDTO.CountId, entityDTO.Sum))
                    return BadRequest(new TransactionEntityDTO { IsSuccessful = false, Notification = "Недостаточно средств на счету для перевода" });

                var senderCount = await countRepository.Get(entityDTO.CountId);
                var reciveCount = await countRepository.Get(entityDTO.CountReciverId);

                if (reciveCount == null) return BadRequest(new TransactionEntityDTO { IsSuccessful = false, Notification = "Не найден счет получателя" });

                TransactionEntity transactionEntity = new TransactionEntity
                {
                    Sum = entityDTO.Sum,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CountId = entityDTO.CountId,
                    CountReciverId = entityDTO.CountReciverId,
                    Description = $"Был выполнен перевод со счета {entityDTO.CountId} на счет {entityDTO.CountReciverId}"
                };

                senderCount.Count -= entityDTO.Sum;
                reciveCount.Count += entityDTO.Sum;

                senderCount = await countRepository.Update(senderCount);
                reciveCount = await countRepository.Update(reciveCount);

                var transaction = await repository.Add(transactionEntity);

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
