using DatingAppAPI.Data;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entites;
using DatingAppAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext dataContext,ITokenService tokenService)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO) {

            if (await UserExists(registerDTO.UserName)) {
                return BadRequest("Username is Taken");
            }

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDTO.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };


            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return new UserDTO {

                Username = registerDTO.UserName,
                Token = _tokenService.CreateToken(user),
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login) {

            var user = await _dataContext.Users.SingleOrDefaultAsync(x => x.UserName == login.Username);

            if (user == null)
            {
                return Unauthorized("Invalid UserName");
            }
            else {
                using var hmac = new HMACSHA512(user.PasswordSalt);

                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != user.PasswordHash[i]) {
                        return Unauthorized("Inavlid Password");
                    }
                }

                return new UserDTO
                {
                    Username = login.Username,
                    Token = _tokenService.CreateToken(user)
                };
            }
        }

        private async Task<bool> UserExists(string username) {

            return await _dataContext.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
