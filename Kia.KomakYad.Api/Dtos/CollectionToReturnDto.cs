using Kia.KomakYad.Domain.Dtos;
using System;

namespace Kia.KomakYad.Api.Dtos
{
    public class CollectionToReturnDto
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public UserDetailedDto Auther { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
