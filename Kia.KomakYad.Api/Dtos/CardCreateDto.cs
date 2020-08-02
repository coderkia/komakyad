using Kia.KomakYad.Api.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Api.Dtos
{
    public class CardCreateDto
    {
        public int Id { get; set; }

        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Required]
        public string Answer { get; set; }

        [Required]
        public string Question { get; set; }

        public string Example { get; set; }

        public ReadCardJsonData JsonData { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CollectionId { get; set; }
    }
}
