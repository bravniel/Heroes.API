using Heroes.API.Models;

namespace Heroes.API.Repositories
{
    public interface IHeroesRepository
    {
        Task<bool> AddHeroToTrainer(int heroId, string userName);
        Task<int> AddNewHero(NewHeroModel newHeroModel);
        Task<List<HeroModel>> GetAllAvailableHeroes();
        Task<List<HeroModel>> GetAllHeroesByUserName(string userName);
        Task<decimal?> TrainHero(int heroId, string userName);
    }
}