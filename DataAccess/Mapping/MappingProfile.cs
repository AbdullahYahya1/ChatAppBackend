using AutoMapper;
using Business.Entities;
using DataAccess.Dtos.ConversationDtos;
using DataAccess.Dtos.ConversationUserDtos;
using DataAccess.Dtos.General;
using DataAccess.Dtos.MessageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           CreateMap<GetConversationDto,Conversation>().ReverseMap();
           CreateMap<GetConversationUserDto, ConversationUser>().ReverseMap();
           CreateMap<GetMessageDto, Message>().ReverseMap();
           CreateMap<GetMessageDto, Message>().ReverseMap();

            CreateMap<User, GetUserDto>().ReverseMap();
            //CreateMap<Area, LookUpDataModel<string>>()
            //.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.AreaId))
            //.ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
            //.ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn));
        }
    }
}
