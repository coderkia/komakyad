using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Api.Dtos
{
    public class CollectionCreateDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }

        [Required]
        [StringLength(450)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        public bool IsPrivate { get; set; }
    }
}
