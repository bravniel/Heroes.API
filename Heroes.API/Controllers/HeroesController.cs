using Heroes.API.Models;
using Heroes.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Heroes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroesRepository _heroesRepository;
        public HeroesController(IHeroesRepository heroesRepository)
        {
            _heroesRepository = heroesRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddNewHero([FromBody] NewHeroModel newHeroModel)
        {
            var res = await _heroesRepository.AddNewHero(newHeroModel);
            if(res == -1)
            {
                return BadRequest();
            }
            return Ok(res);
        }

        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> GetAllAvailableHeroes()
        {
            var res = await _heroesRepository.GetAllAvailableHeroes();
            if (res == null || res.Count == 0)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpPost("add-hero-to-trainer/{heroId:int}")]
        public async Task<IActionResult> AddHeroToTrainer([FromRoute] int heroId)
        {
            var userName = User.Identity.Name;
            if(userName == null)
            {
                return BadRequest();
            }
            var res = await _heroesRepository.AddHeroToTrainer(heroId, userName);
            if (!res)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("all-mine-heroes")]
        [Authorize]
        public async Task<IActionResult> GetAllMyHeroes()
        {
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            var res = await _heroesRepository.GetAllHeroesByUserName(userName);
            if (res == null || res.Count == 0)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpPost("train/{heroId:int}")]
        public async Task<IActionResult> TrainMyHero([FromRoute] int heroId)
        {
            var userName = User?.Identity?.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            var res = await _heroesRepository.TrainHero(heroId, userName);
            if (res == null)
            {
                return BadRequest();
            }
            return Ok(res);
        }
    }
}
