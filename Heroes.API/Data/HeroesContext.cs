using Heroes.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Heroes.API.Data
{
    public class HeroesContext : IdentityDbContext<TrainerModel>
    {
        public HeroesContext(DbContextOptions<HeroesContext> options) : base(options)
        {
        }
        public DbSet<HeroModel> Heroes { get; set; }
        public DbSet<SuitColorModel> SuitColors { get; set; }
    }
}
