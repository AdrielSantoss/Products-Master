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
    }
}
