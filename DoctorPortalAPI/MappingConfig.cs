using AutoMapper;
using DoctorPortalAPI.Models;
using DoctorPortalAPI.Models.Dto;

namespace DoctorPortalAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
