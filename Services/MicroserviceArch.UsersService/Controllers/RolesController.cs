using MicroserviceArch.DAL.Entities;
using MicroserviceArch.DTOEntity;
using MicroserviceArch.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceArch.UsersService.Controllers
{
    /// <summary>
    /// Контроллер ролей
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository<RoleEntity> repository;

        public RolesController(IRoleRepository<RoleEntity> repository) => this.repository = repository;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var role = await repository.Get(id);

            if (role is null) return NotFound(new RoleEntityDTO { IsSuccesful = false, Notification = "Запись не найдена" });

            RoleEntityDTO roleEntityDTO = new RoleEntityDTO
            {
                IsSuccesful = true,
                Id = role.Id,
                Name = role.Name,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt,
                Description = role.Description,
            };

            return Ok(roleEntityDTO);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() => Ok(await repository.GetAll());

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] RoleEntityDTO roleEntityDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RoleEntity roleEntity = new RoleEntity()
                    {
                        Name = roleEntityDTO.Name,
                        Description = roleEntityDTO.Description,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    var result = await repository.Add(roleEntity);

                    roleEntityDTO.Id = result.Id;
                    roleEntityDTO.IsSuccesful = true;

                    return Ok(roleEntityDTO);
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                    return BadRequest(new RoleEntityDTO { IsSuccesful = false, Notification = $"{errors}" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new RoleEntityDTO { IsSuccesful = false, Notification = $"{ex.Message}" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]RoleEntityDTO item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                    return BadRequest(new RoleEntityDTO { IsSuccesful = false, Notification = $"{errors}" });
                }

                var role = await repository.Get(item.Id);

                if(role is null)
                {
                    item.IsSuccesful = false;
                    item.Notification = $"Не удалось найти роль {item.Name} с иднексом {item.Id} для изменения";
                    return NotFound(item);
                }

                role.Description = item.Description;
                role.UpdatedAt = DateTime.UtcNow;
                role.Name = item.Name;

                var result = await repository.Update(role);

                item.Name = result.Name;
                item.Description = result.Description;
                item.UpdatedAt = result.UpdatedAt;
                item.CreatedAt = result.CreatedAt;
                item.IsSuccesful = true;

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
                    return NotFound(new RoleEntityDTO
                    {
                        IsSuccesful = false,
                        Notification = "Роль для удаления не найдена"
                    });

                return Ok(new RoleEntityDTO
                {
                    Name = result.Name,
                    Description = result.Description,
                    CreatedAt = result.CreatedAt,
                    UpdatedAt = result.UpdatedAt,
                    IsSuccesful = true,
                    Id = result.Id
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new RoleEntityDTO { IsSuccesful = false, Notification = $"{ex.Message}" });
            }
        }
    }
}
