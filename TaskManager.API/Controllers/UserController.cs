﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Data.Repository;
using TaskManager.API.Data.Repository.UserRepo;
using TaskManager.API.Dto.User;
using TaskManager.API.Helpers.Filters;
using TaskManager.API.Model;

namespace TaskManager.API.Controllers
{
    [UserAuthorizationFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public UserController(IRepositoryWrapper repositoryWrapper, IMapper mapper, UserManager<User> userManager)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
            this.userManager = userManager;
        }  

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound("Could not find user.");
            }

            var userForReturn = mapper.Map<UserForReturn>(user);

            return Ok(userForReturn);
        }

        [HttpPut("{userId}/photo/{photoId}")]
        public async Task<IActionResult> ChangePhoto(int userId, int photoId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound("Could not find user.");
            }

            user.PhotoId = photoId;

            if (await repositoryWrapper.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Could not change photo.");
        }

        [HttpPut("{userId}/changeNick")]
        public async Task<IActionResult> ChangeNick(int userId, UserForChangeNick userForChangeNick)
        {
            var getUser = await userManager.FindByIdAsync(userId.ToString());

            if (getUser == null)
            {
                return NotFound("Could not find user.");
            }

            var checkNick = await repositoryWrapper.UserRepository.GetUserByNick(userForChangeNick.Nickname.ToLower());

            if (checkNick != null)
            {
                return BadRequest("User with this nickname currently exists.");
            }

            getUser.Nickname = userForChangeNick.Nickname.ToLower();

            if (await repositoryWrapper.SaveAll())
            {
                var userForReturn = await userManager.FindByIdAsync(userId.ToString());

                var user = mapper.Map<UserForReturnNickname>(userForReturn);

                return Ok(new
                {
                    user
                });
            }

            return BadRequest("Could not change nickname.");
        }

        [HttpPut("{userId}/changePassword")]
        public async Task<IActionResult> ChangePassword(int userId, UserForChangePassword userForChangePassword)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound("Could not find user.");
            }

            var changePassword = await userManager.ChangePasswordAsync(user, userForChangePassword.CurrentPassword, userForChangePassword.Password);

            if (changePassword.Succeeded)
            {
                return Ok();
            }  
            
            return Unauthorized();        
        }

    }
}