using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Data.Repository;
using TaskManager.API.Data.Repository.UserRepo;
using TaskManager.API.Dto.User;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IMainRepository mainRepository;

        public UserController(IUserRepository userRepository, IMapper mapper, IMainRepository mainRepository)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.mainRepository = mainRepository;
        }  

        [HttpPut("{userId}/photo/{photoId}")]
        public async Task<IActionResult> ChangePhoto(int userId, int photoId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var user = await userRepository.GetUser(userId);

            if (user == null)
            {
                return NotFound("Could not find user.");
            }

            user.PhotoId = photoId;

            if (await mainRepository.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Could not change photo.");
        }
    }
}