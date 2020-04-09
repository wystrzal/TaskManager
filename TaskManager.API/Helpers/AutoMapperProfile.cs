using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Dto;
using TaskManager.API.Dto.Message;
using TaskManager.API.Dto.Project;
using TaskManager.API.Dto.User;
using TaskManager.API.Model;

namespace TaskManager.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //User
            CreateMap<UserForRegister, User>();

            CreateMap<User, UserForUserDetail>();

            //Message
            CreateMap<MessageForAdd, Message>();

            CreateMap<Message, MessageForReturnReceivedMessages>()
                .ForMember(dest => dest.From,
                opt => opt.MapFrom(src => src.Sender.Nickname));

            CreateMap<Message, MessageForReturnSendedMessages>()
                .ForMember(dest => dest.To,
                opt => opt.MapFrom(src => src.Recipient.Nickname));

            CreateMap<Message, MessageForReturnDetailMessage>()
                .ForMember(dest => dest.To,
                opt => opt.MapFrom(src => src.Recipient.Nickname))
                .ForMember(dest => dest.From,
                opt => opt.MapFrom(src => src.Sender.Nickname));

            //Project
            CreateMap<ProjectForAdd, Project>();

            CreateMap<Project, ProjectForReturn>()
                 .ForMember(dest => dest.AnyUsers,
                 opt => opt.MapFrom(src => src.UserProjects.Where(up => up.Status == "active").ToList().Count > 1 ? true : false));

            CreateMap<Project, ProjectForReturnInvitations>();
                       
        }
    }
}
