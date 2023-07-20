using MicroserviceArch.DAL.Entities;
using MicroserviceArch.DTOEntity;
using MicroserviceArch.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceArch.CountsService.Controllers
{
    /// <summary>
    /// Контроллер счетов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CountsController : ControllerBase
    {
        private readonly ICountRepository<CountEntity> repository;

        public CountsController(ICountRepository<CountEntity> repository) => this.repository = repository;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<CountEntityDTO> counts = new List<CountEntityDTO>();

            foreach(var count in await repository.GetAll())
            {
                counts.Add(new CountEntityDTO()
                {
                    Client = count.Client.Name,
                    Count = count.Count,
                    Id = count.Id,
                    IsSuccessful = true,
                    ClientId = count.Client.Id,
                    CreatedAt = count.CreatedAt,
                    UpdatedAt = count.UpdatedAt,
                });
            }

            return Ok(counts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var count = await repository.Get(id);

            if (count is null) return NotFound(new CountEntityDTO { IsSuccessful = false, Notification = "Запись не найдена" });

            CountEntityDTO countEntityDTO = new CountEntityDTO
            {
                IsSuccessful = true,
                Id = count.Id,
                Count = count.Count,
                Client = count.Client.Name,
                CreatedAt = count.CreatedAt,
                UpdatedAt = count.UpdatedAt,
                ClientId=count.Client.Id
            };

            return Ok(countEntityDTO);
        }

        [HttpGet("/client-counts/{id}")]
        public async Task<IActionResult> CheckCounts(int id)
        {
            List<CountEntityDTO> countEntities = new List<CountEntityDTO>();

            if(!await repository.ExistsCounts(id)) 
            {
                return Ok(new CountEntityDTO { IsSuccessful = false, Notification = "Нет счетов" });
            }

            foreach(var count in await repository.GetAllByUser(id)) 
            {
                countEntities.Add(new CountEntityDTO
                {
                    IsSuccessful = true,
                    Count = count.Count,
                    UpdatedAt = count.UpdatedAt,
                    CreatedAt = count.CreatedAt,
                    Client = count.Client.Name,
                    ClientId = count.ClientId,
                    Id = count.Id
                });
            }

            return Ok(countEntities);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CountEntityDTO countEntityDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                    return BadRequest(new CountEntityDTO { IsSuccessful = false, Notification = $"{errors}" });
                }

                CountEntity count = new CountEntity()
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ClientId=countEntityDTO.ClientId,
                    Count = countEntityDTO.Count,
                };

                var result = await repository.Add(count);

                countEntityDTO.IsSuccessful = true;
                countEntityDTO.Id = result.Id;
                countEntityDTO.Count = result.Count;
                countEntityDTO.CreatedAt = result.CreatedAt;
                countEntityDTO.UpdatedAt = result.UpdatedAt;
                countEntityDTO.ClientId = result.ClientId;
                countEntityDTO.Client = result.Client.Name;

                return Ok(countEntityDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new CountEntityDTO { IsSuccessful = false, Notification = $"{ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await repository.Delete(id);

                if (result is null)
                    return NotFound(new CountEntityDTO
                    {
                        IsSuccessful = false,
                        Notification = "Счет для удаления не найден"
                    });

                return Ok(new CountEntityDTO
                {
                    IsSuccessful = true,
                    Id = result.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new CountEntityDTO { IsSuccessful = false, Notification = $"{ex.Message}" });
            }
        }
    }
}
