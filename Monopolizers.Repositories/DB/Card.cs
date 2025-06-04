using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monopolizers.Repository.DB
{
    [Table("Card")]

    public class Card
    {
        [Key]
        public int CardId { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? AvtImgUrl { get; set; }
        public string? ARVideoUrl { get; set; }
        public int Category { get; set; }

        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public TypeCard Type { get; set; }

        public string? DefaultData { get; set; }
        public string AccessLevel { get; set; }
        public ICollection<Design> Designs { get; set; }
    }
}
