using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IRepository _repo;
        public UsersController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> postUser(User model)
        {
            try
            {
                 _repo.Add(model);
                if (await _repo.SaveChangesAsync())
                {
                    return Ok(model);
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro: {e.Message}");
            }
            return BadRequest();

        }
        [HttpGet]
        public async Task<IActionResult> getUsers()
        {
            try
            {
                var result = await _repo.GetAllUsers();
                return Ok(result);
            }
            catch (Exception e)
            {

                return BadRequest($"Erro: {e.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getUserById(int id)
        {
            try
            {
                var result = await _repo.GetUserById(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro: {e.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> updateUserById(int id, User model)
        {
            try
            {
                var user = await _repo.GetUserById(id);
                if (user == null) return BadRequest("User não encontrado");

                _repo.Update(model);

                if(await _repo.SaveChangesAsync())
                {
                    return Ok(model);
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro: {e.Message}");
            }
            return BadRequest("Erro inesperado");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteUser(int id)
        {
            try
            {
                var user = await _repo.GetUserById(id);
                if (user == null) return BadRequest("Usuário não encontrado");
                _repo.Delete(user);
                if( await _repo.SaveChangesAsync())
                {
                    return Ok("Deletado");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}");
            }
            return BadRequest("Erro inesperado");
        }
    }
}
