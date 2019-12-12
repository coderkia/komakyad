using AutoMapper;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Dtos;

namespace Kia.KomakYad.Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDetailedDto>();
            CreateMap<User, UserListDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<UserForRegisterDto, User>();
        }
    }
}
