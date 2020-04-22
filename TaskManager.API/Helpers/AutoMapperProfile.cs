using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Dto;
using TaskManager.API.Dto.Message;
using TaskManager.API.Dto.Project;
using TaskManager.API.Dto.Task;
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

            CreateMap<UserForChangeNick, User>();

            CreateMap<UserForChangePassword, User>();

            CreateMap<User, UserForReturnNickname>();

            CreateMap<User, UserForReturn>();

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

            CreateMap<Project, ProjectForReturnAdded>();

            CreateMap<Project, ProjectForReturn>()
                 .ForMember(dest => dest.AnyUsers,
                 opt => opt.MapFrom(src => src.UserProjects.Where(up => up.Status == "active").ToList().Count > 1 ? true : false))
                 .ForMember(dest => dest.ProjectUsersNick,
                 opt => opt.MapFrom(src => src.UserProjects.Select(up => up.User.Nickname)));

            CreateMap<Project, ProjectForReturnInvitations>();

            CreateMap<ProjectForChangeName, Project>();

            //Task 
            CreateMap<TaskForAdd, PTask>();

            CreateMap<PTask, TaskForReturn>()
                .ForMember(dest => dest.TimeToEnd,
                opt => opt.MapFrom(src => src.TimeToEnd.Subtract(DateTime.Now).TotalDays))
                .ForMember(dest => dest.TaskOwner,
                opt => opt.MapFrom(src => src.Owner))
                .ForMember(dest => dest.TaskId,
                opt => opt.MapFrom(src => src.PTaskId))
                .ForMember(dest => dest.TaskOwnerPhoto,
                opt => opt.MapFrom(src => src.Project.UserProjects.Where(up => up.UserId == src.Owner)
                    .Select(up => up.User.PhotoId).FirstOrDefault()));

            CreateMap<PTask, TaskForReturnImportant>()
                .ForMember(dest => dest.TimeToEnd,
                opt => opt.MapFrom(src => src.TimeToEnd.Subtract(DateTime.Now).TotalDays))
                .ForMember(dest => dest.ProjectName,
                opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.ProjectId,
                opt => opt.MapFrom(src => src.Project.ProjectId))
                .ForMember(dest => dest.TaskId,
                opt => opt.MapFrom(src => src.PTaskId));

        }
   }
}
