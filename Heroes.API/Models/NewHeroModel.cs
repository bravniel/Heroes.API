using Heroes.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Heroes.API.Models
{
    public class NewHeroModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public HeroAbility HeroAbility { get; set; }
        [Required]
        public IList<int> SuitColorsIds { get; set; }
        [Required]
        [Precision(38, 2)]
        public decimal StartingPower { get; set; }
    }
}
