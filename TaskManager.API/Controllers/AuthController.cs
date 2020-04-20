using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManager.API.Dto;
using TaskManager.API.Dto.User;
using TaskManager.API.Helpers;
using TaskManager.API.Model;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private Random random = new Random();

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
            this.mapper = mapper;

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLogin userForLoginDto)
        {
            var dbUser = await userManager.FindByNameAsync(userForLoginDto.Username);

            if (dbUser == null)
            {
                return Unauthorized();
            }      

            var result = await signInManager.CheckPasswordSignInAsync(dbUser, userForLoginDto.Password, false);

            if (result.Succeeded)
            {
                var userToReturn = await userManager.Users.FirstOrDefaultAsync(u => u.Id == dbUser.Id);

                var user = mapper.Map<UserForReturnNickname>(userToReturn);

                return Ok(new
                {
                    token = GenerateToken.GenerateJwtToken(userToReturn, config),
                    user
                });
            }

            return Unauthorized();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister userForRegisterDto)
        {
            if (userForRegisterDto.Password != userForRegisterDto.RepeatPassword)
            {
                return BadRequest("The password repeat is incorrect.");
            }

            var userToCreate = mapper.Map<User>(userForRegisterDto);

            var lastUser = userManager.Users.OrderByDescending(u => u.Id).First();

            if (lastUser == null)
            {
                userToCreate.Nickname = "user" + 1;
            }

            userToCreate.Nickname = "user" + lastUser.Id;

            userToCreate.PhotoId = random.Next(1, 9);

            var result = await userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest("User with this login already exists.");

        }

    }
}
