using Heroes.API.Data;
using Heroes.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Heroes.API.Repositories
{
    public class HeroesRepository : IHeroesRepository
    {
        private readonly HeroesContext _heroesContext;
        public HeroesRepository(HeroesContext heroesContext)
        {
            _heroesContext = heroesContext;
        }
        public async Task<int> AddNewHero(NewHeroModel newHeroModel)
        {
            HeroModel heroModel = new()
            {
                Name = newHeroModel.Name,
                HeroAbility = newHeroModel.HeroAbility,
                SuitColors = await SuitColorsConventer(newHeroModel.SuitColorsIds),
                StartingPower = newHeroModel.StartingPower,
                CurrentPower = newHeroModel.StartingPower
            };

            _heroesContext.Heroes.Add(heroModel);
            var res = await _heroesContext.SaveChangesAsync();
            if (res != 0)
            {
                return heroModel.Id;
            }
            return -1;
        }
        private async Task<IList<SuitColorModel>> SuitColorsConventer(IList<int> suitColorsIds)
        {
            IList<SuitColorModel> suitColors = null;
            foreach (var suitColorId in suitColorsIds)
            {
                var suitColor = await _heroesContext.SuitColors.Include(s => s.Hero).FirstOrDefaultAsync(s => s.Id == suitColorId);
                if (suitColor != null)
                {
                    suitColors ??= new List<SuitColorModel>();
                    suitColors.Add(suitColor);
                }
            }
            return suitColors;
        }

        public async Task<List<HeroModel>> GetAllAvailableHeroes()
        {
            var heroes = await _heroesContext.Heroes.Include(h => h.Trainer).Include(h => h.SuitColors).Where(h => h.Trainer == null).ToListAsync();
            return heroes;
        }

        public async Task<bool> AddHeroToTrainer(int heroId, string userName)
        {
            var user = await _heroesContext.Users.Include(u => u.Heroes).FirstOrDefaultAsync(u => u.Email == userName);
            if (user == null)
            {
                return false;
            }
            var hero = await _heroesContext.Heroes.Include(h => h.Trainer).Include(h => h.SuitColors).FirstOrDefaultAsync(h => h.Id == heroId);
            if (hero == null)
            {
                return false;
            }
            if (user.Heroes.Contains(hero))
            {
                return false;
            }
            user.Heroes.Add(hero);
            var res = await _heroesContext.SaveChangesAsync();
            return res != 0;
        }

        public async Task<List<HeroModel>> GetAllHeroesByUserName(string userName)
        {
            var user = await _heroesContext.Users.Include(u => u.Heroes).FirstOrDefaultAsync(u => u.Email == userName);
            if (user == null)
            {
                return null;
            }
            return user.Heroes.OrderByDescending(h => h.CurrentPower).ToList();
        }

        public async Task<decimal?> TrainHero(int heroId, string userName)
        {
            var user = await _heroesContext.Users.Include(u => u.Heroes).FirstOrDefaultAsync(u => u.Email == userName);
            if (user == null)
            {
                return null;
            }
            var hero = await _heroesContext.Heroes.FindAsync(heroId);
            if (hero == null || !user.Heroes.Contains(hero))
            {
                return null;
            }
            if (!hero.LastTrainingDate.HasValue)
            {
                TrainHeroHandler(ref hero, false);
            }
            else if (hero.LastTrainingDate.HasValue && hero.LastTrainingDate.Value.Date < DateTime.Today.Date)
            {
                TrainHeroHandler(ref hero, false);
            }
            else if (hero.DailyTrainingCount.HasValue && hero.DailyTrainingCount.Value < 5)
            {
                TrainHeroHandler(ref hero, true);
            }
            var res = await _heroesContext.SaveChangesAsync();
            if (res != 0)
            {
                return hero.CurrentPower;
            }
            return null;
        }
        private void TrainHeroHandler(ref HeroModel hero, bool isSameDay)
        {
            hero.CurrentPower = TrainHeroResult(hero.CurrentPower);
            hero.LastTrainingDate = DateTime.Today.Date;
            hero.DailyTrainingCount = isSameDay ? hero.DailyTrainingCount + 1 : 1;
        }
        private decimal TrainHeroResult(decimal currentPower)
        {
            Random random = new();
            int rand = random.Next(100, 110);
            decimal newPower = currentPower * rand / 100;
            return newPower;
        }
    }
}
