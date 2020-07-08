using Kia.KomakYad.Domain.Dtos;
using System;

namespace Kia.KomakYad.Api.Dtos
{
    public class CollectionToReturnDto
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public UserDetailedDto Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public int CardsCount { get; set; }
        public int FollowersCount { get; set; }
    }
}
