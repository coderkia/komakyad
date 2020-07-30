using AutoMapper;
using Kia.KomakYad.Api.Dtos;
using Kia.KomakYad.Api.Models;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Domain.Dtos;
using Newtonsoft.Json;

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
            CreateMap<CardCreateDto, Card>();
            CreateMap<Card, CardToReturnDto>();
            CreateMap<Card, CardToSearchReturnDto>();
            CreateMap<Card, ReadCard>()
                .ForMember(m => m.CardId, opt => opt.MapFrom(u => u.Id));
            CreateMap<Collection, CollectionToReturnDto>();
            CreateMap<CollectionCreateDto, Collection>();
            CreateMap<ReadCard, ReadCardToReturnDto>()
                .ForMember(m => m.JsonData, opt => opt.MapFrom(u => JsonConvert.DeserializeObject<ReadCardJsonData>(u.JsonData)));
            CreateMap<ReadCollection, ReadCollectionToReturnDto>();
        }
    }
}
