using API.Dtos;
using API.Identity;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IRepository _repo;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthController(IRepository repo, IConfiguration config, UserManager<UserIdentity> userManager, SignInManager<UserIdentity> signInManager, IMapper mapper)
        {
            _repo = repo;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._mapper = mapper;
            this._config = config;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(UserDto userDto)
        {
            return Ok(userDto);
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<UserIdentity>(userDto);
                var userToReturn = _mapper.Map<UserDto>(user);
                var userCompareEmail = await _userManager.FindByEmailAsync(userDto.Email);

                if (userCompareEmail != null)
                {
                    return BadRequest("Email já cadastrado");
                }else
                {
                    var result = await _userManager.CreateAsync(user, userDto.Password);
                    if (result.Succeeded)
                    {
                        return Created("GetUser", userToReturn);
                    }

                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous] //nao precisa ser authenticado, ou seja, nao se aplica a policy (criada no startup)
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email); //busca pelo email o usupario no banco

                var result = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false); //verifica se o usuário tem o mesmo password
                if (result.Succeeded)
                {
                    var appuser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == userDto.Email.ToUpper());

                    var userToReturn = _mapper.Map<UserLoginDto>(appuser);

                    return Ok(new { 
                        token = GenerateJWToken(appuser).Result,
                        user = userToReturn
                    });
                }
                return Unauthorized();
            }
            catch (Exception e )
            {

                return BadRequest(e.Message);
            }
        }

        private async Task<string> GenerateJWToken(UserIdentity user)
        {
            var claims = new List<Claim>            //Claims: reividincação de autorização 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), //criando a primeira questão de autorização (primeira Claim)
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user); //busca os papeis que o usuário logou possui, (Administrador, gerente e etc)

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); //para cada role do usuário adiciona na claim

            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("appSettings:Token").Value)); //com o token da api, será realizado a criptografia

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); //algoritimo de criptografia

            var tokenDescriptor = new SecurityTokenDescriptor //passando descrição do token
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), //expiração do token
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor); //criando o token com a descrição passado pro tokenDescriptor

            return tokenHandler.WriteToken(token);

        }
    }
}
