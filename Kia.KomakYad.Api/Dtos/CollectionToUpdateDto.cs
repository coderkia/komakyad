using Kia.KomakYad.DataAccess.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Api.Dtos
{
    public class CollectionToUpdateDto
    {
        [Required]
        [StringLength(450)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        public void Map(Collection collection)
        {
            collection.Title = Title;
            collection.Description = Description;
            collection.ModifiedOn = DateTime.Now;
        }
    }
}
