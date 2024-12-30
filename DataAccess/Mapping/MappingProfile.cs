using AutoMapper;
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

           //CreateMap<Area, LookUpDataModel<string>>()
           //.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.AreaId))
           //.ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
           //.ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn));
        }
    }
}
