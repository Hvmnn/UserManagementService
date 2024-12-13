using AutoMapper;
using UserManagementService.Src.DTOs;
using UserManagementService.Src.Models;

namespace UserManagementService.Src.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Career, CareerDto>();
            CreateMap<User, UserDto>();
        }
    }
}