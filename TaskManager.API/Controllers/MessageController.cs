using System;
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
    [Route("api/user/{userId}/[controller]")]
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

        [HttpPost("send")]
        public async Task<IActionResult> AddMessage(int userId, [FromQuery]string recipientNick, MessageForAdd messageForAddDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var recipient = await userRepository.GetUserByNick(recipientNick);

            if (recipient == null)
                return NotFound("User with this nickname do not exist.");

            var messageToAdd = mapper.Map<Message>(messageForAddDto);

            messageToAdd.SenderId = userId;
            messageToAdd.RecipientId = recipient.Id;

            mainRepository.Add(messageToAdd);

            if (await mainRepository.SaveAll())
                return Ok();

            return BadRequest("Could not send the message.");
        }

        [HttpGet("received")]
        public async Task<IActionResult> GetReceivedMessages(int userId, [FromQuery]int skip)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var receivedMessages = await messageRepository.GetReceivedMessages(userId, skip);

            var messageForReturn = mapper.Map<IEnumerable<MessageForReturnReceivedMessages>>(receivedMessages);

            return Ok(messageForReturn);
        }

        [HttpGet("sended")]
        public async Task<IActionResult> GetSendedMessages(int userId, [FromQuery]int skip)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var receivedMessages = await messageRepository.GetSendedMessages(userId, skip);

            var messageForReturn = mapper.Map<IEnumerable<MessageForReturnSendedMessages>>(receivedMessages);

            return Ok(messageForReturn);
        }

        [HttpGet("{messageId}")]
        public async Task<IActionResult> GetMessage(int messageId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await messageRepository.GetMessage(messageId, userId);

            if (message == null)
                return NotFound("Could not find the message");

            var messageForReturn = mapper.Map<MessageForReturnDetailMessage>(message);

            return Ok(messageForReturn);
        }

        [HttpPost("delete/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId, int userId, [FromQuery]string userType)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await messageRepository.GetMessage(messageId, userId);

            if (message == null)
                return NotFound("Could not find the message");

            if (userType == "recipient")
            {
                message.RecipientDeleted = true;
            }
            else
            {
                message.SenderDeleted = true;
            }

            await mainRepository.SaveAll();

            if(message.SenderDeleted == true && message.RecipientDeleted == true)
            {
                mainRepository.Delete(message);

                if (await mainRepository.SaveAll())
                    return Ok();

                return BadRequest("Could not delete the message.");
            }

            return NoContent();
        }

    }
}