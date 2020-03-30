using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Dto;
using TaskManager.API.Dto.Message;
using TaskManager.API.Dto.User;
using TaskManager.API.Model;

namespace TaskManager.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserForRegisterDto, User>();

            CreateMap<User, UserForUserDetailDto>();

            CreateMap<MessageForAddDto, Message>();

            CreateMap<Message, MessageForReturnReceived>()
                .ForMember(dest => dest.From,
                opt => opt.MapFrom(src => src.Sender.Nickname));

            CreateMap<Message, MessagesForReturnSended>()
                .ForMember(dest => dest.To,
                opt => opt.MapFrom(src => src.Recipient.Nickname));
        }
    }
}
