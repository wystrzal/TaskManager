﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Data.Repository;
using TaskManager.API.Data.Repository.MessageRepo;
using TaskManager.API.Data.Repository.UserRepo;
using TaskManager.API.Dto.Message;
using TaskManager.API.Model;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMainRepository mainRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public MessageController(IMainRepository mainRepository, IMessageRepository messageRepository,
            IUserRepository userRepository, IMapper mapper)
        {
            this.mainRepository = mainRepository;
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpPost("{senderId}")]
        public async Task<IActionResult> AddMessage(int senderId, [FromQuery]string recipientNick, MessageForAddDto messageForAddDto)
        {
            if (senderId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var recipient = await userRepository.GetUserByNick(recipientNick);

            if (recipient == null)
                return BadRequest("User with this nickname do not exist.");

            var messageToAdd = mapper.Map<Message>(messageForAddDto);

            messageToAdd.SenderId = senderId;
            messageToAdd.RecipientId = recipient.Id;

            mainRepository.Add(messageToAdd);

            if (await mainRepository.SaveAll())
                return Ok();

            return BadRequest("Could not send the message.");
        }

        [HttpGet("received/{userId}")]
        public async Task<IActionResult> GetReceivedMessages(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var receivedMessages = await messageRepository.GetReceivedMessages(userId);

            //To change
            if (receivedMessages == null)
                return BadRequest("Error");

            var messageForReturn = mapper.Map<IEnumerable<MessageForReturnReceived>>(receivedMessages);

            return Ok(messageForReturn);   
        }

        [HttpGet("sended/{userId}")]
        public async Task<IActionResult> GetSendedMessages(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var receivedMessages = await messageRepository.GetSendedMessages(userId);

            //To change
            if (receivedMessages == null)
                return BadRequest("Error");

            var messageForReturn = mapper.Map<IEnumerable<MessageForReturnReceived>>(receivedMessages);

            return Ok(messageForReturn);
        }
    }
}