using MicroserviceArch.DAL.Entities;
using MicroserviceArch.DTOEntity;
using MicroserviceArch.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace MicroserviceArch.UsersService.Controllers
{
    /// <summary>
    /// Контроллер клиентов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository<ClientEntity> repository;

        public ClientsController(IClientRepository<ClientEntity> repository) => this.repository = repository;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var client = await repository.Get(id);

            if (client is null) return NotFound(new ClientEntityDTO { IsSuccesfull = false, Notification = "Запись не найдена" });

            ClientEntityDTO clientEntityDTO = new ClientEntityDTO
            {
                IsSuccesfull = true,
                Id = client.Id,
                Name = client.Name,
                CreatedAt = client.CreatedAt,
                UpdatedAt = client.UpdatedAt,
                Email = client.Email,
                RoleId = client.RoleId,
            };

            return Ok(clientEntityDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            List<ClientEntityDTO> clients = new List<ClientEntityDTO>();

            foreach(var client in await repository.GetAll())
            {
                clients.Add(new ClientEntityDTO
                {
                    CreatedAt = client.CreatedAt,
                    UpdatedAt = client.UpdatedAt,
                    RoleId = client.RoleId,
                    Id = client.Id,
                    Email = client.Email,
                    Name = client.Name,
                    IsSuccesfull = true,
                });
            }

            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ClientEntityDTO clientEntityDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                    return BadRequest(new RoleEntityDTO { IsSuccesful = false, Notification = $"{errors}" });
                }

                ClientEntity client = new ClientEntity()
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Email = clientEntityDTO.Email,
                    Password = clientEntityDTO.Password,
                    Name = clientEntityDTO.Name,
                    RoleId = clientEntityDTO.RoleId,
                };

                var result = await repository.Add(client);

                clientEntityDTO.IsSuccesfull = true;
                clientEntityDTO.Id = result.Id;
                clientEntityDTO.Password = string.Empty;

                return Ok(clientEntityDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new RoleEntityDTO { IsSuccesful = false, Notification = $"{ex.Message}" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClientEntityDTO item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                    return BadRequest(new ClientEntityDTO { IsSuccesfull = false, Notification = $"{errors}" });
                }

                var client = await repository.Get(item.Id);

                if (client is null)
                {
                    item.IsSuccesfull = false;
                    item.Notification = $"Не удалось найти клиента {item.Name} с иднексом {item.Id} для изменения";
                    return NotFound(item);
                }

                client.Name = item.Name;
                client.UpdatedAt = DateTime.UtcNow;
                client.Password = item.Password;
                client.Email = item.Email;
                client.RoleId=item.RoleId;

                var result = await repository.Update(client);

                item.Name = result.Name;
                item.CreatedAt = result.CreatedAt;
                item.UpdatedAt = result.UpdatedAt;
                item.Password = string.Empty;
                item.Email = result.Email;
                item.RoleId = result.RoleId;

                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(new RoleEntityDTO { IsSuccesful = false, Notification = $"{ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await repository.Delete(id);

                if (result is null)
                    return NotFound(new ClientEntityDTO
                    {
                        IsSuccesfull = false,
                        Notification = "Клиент для удаления не найден"
                    });

                return Ok(new RoleEntityDTO
                {
                    IsSuccesful = true,
                    Id = result.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new RoleEntityDTO { IsSuccesful = false, Notification = $"{ex.Message}" });
            }
        }
    }
}
