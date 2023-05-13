using System.ComponentModel.DataAnnotations;

namespace Heroes.API.Models
{
    public class SuitColorModel
    {
        public int Id { get; set; }
        [Required]
        public string Color { get; set; }
        public HeroModel? Hero { get; set; }
    }
}
