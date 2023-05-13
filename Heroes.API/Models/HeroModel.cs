using Heroes.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Heroes.API.Models
{
    public class HeroModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public HeroAbility HeroAbility { get; set; }
        public DateTime? StartTrainingDate { get; set; }
        [Required]
        public IList<SuitColorModel> SuitColors { get; set; }
        [Required]
        [Precision(38, 2)]
        public decimal StartingPower { get; set; }
        [Precision(38, 2)]
        public decimal CurrentPower { get; set; }
        public TrainerModel? Trainer { get; set; }
        public DateTime? LastTrainingDate { get; set; }
        public int? DailyTrainingCount { get; set; }
    }
}
