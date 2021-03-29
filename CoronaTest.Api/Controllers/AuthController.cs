using Microsoft.Extensions.Configuration;
using CoronaTest.Api.Dtos;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using CoronaTest.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTest.Api.Controllers
{
    /// <summary>
    /// API-Controller für die Autorisierung
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private static List<AuthUser> _users = new List<AuthUser>
        {
          new AuthUser { Email = "admin@htl.at", Password=AuthUtils.GenerateHashedPassword("admin"), UserRole = "Admin" },
          new AuthUser { Email = "user@htl.at", Password=AuthUtils.GenerateHashedPassword("user"), UserRole = "User" }
        };

        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructor mit DI
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        public AuthController(IUnitOfWork unitOfWork, ILogger<AuthController> logger,
            IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _config = config;
        }

        [Route("login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(AuthUserDto userDto)
        {
            var authUser = await _unitOfWork.AuthUsers.GetByEmailAsync(userDto.Email);
            if (authUser == null)
            {
                return Unauthorized();
            }

            if (!AuthUtils.VerifyPassword(userDto.Password, authUser.Password))
            {
                return Unauthorized();
            }

            var tokenString = GenerateJwtToken(authUser);

            IActionResult response = Ok(new
            {
                auth_token = tokenString,
                userMail = authUser.Email,
            });

            return response;
        }

        /// <summary>
        /// Neuen Benutzer registrieren. Bekommt noch keine Rolle zugewiesen.
        /// </summary>
        /// <param name="eMail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostNewUser(string eMail, string password)
        {
            if (string.IsNullOrEmpty(eMail) || string.IsNullOrEmpty(password))
            {
                return BadRequest();
            }

            AuthUser userInDb = await _unitOfWork.AuthUsers.GetByEmailAsync(eMail);
            if(userInDb != null)
            {
                return BadRequest();
            }

            var newAuthUser = new AuthUser
            {
                Email = eMail,
                Password = AuthUtils.GenerateHashedPassword(password),
                UserRole = "User"
            };

            try
            {
                await _unitOfWork.AuthUsers.AddAsync(newAuthUser);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            //_users.Add(newAuthUser);
            return NoContent();
        }

        /// <summary>
        /// JWT erzeugen. Minimale Claim-Infos: Email und Rolle
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>Token mit Claims</returns>
        private string GenerateJwtToken(AuthUser userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>();
            authClaims.Add(new Claim(ClaimTypes.Email, userInfo.Email));
            //authClaims.Add(new Claim(ClaimTypes.Country, "Austria"));
            if (!string.IsNullOrEmpty(userInfo.UserRole))
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userInfo.UserRole));
            }

            var token = new JwtSecurityToken(
              issuer: _config["Jwt:Issuer"],
              audience: _config["Jwt:Audience"],
              claims: authClaims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
