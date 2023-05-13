using Microsoft.AspNetCore.Identity;

namespace Heroes.API.Models
{
    public class TrainerModel : IdentityUser
    {
        public string Name { get; set; }
        public IList<HeroModel>? Heroes { get; set; }
    }
}
