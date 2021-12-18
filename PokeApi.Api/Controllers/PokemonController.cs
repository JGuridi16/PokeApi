using Microsoft.AspNetCore.Mvc;
using PokeApi.Services.Services;
using System.Threading.Tasks;

namespace PokeApi.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PokemonController : Controller
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet("{top}")]
        public async Task<IActionResult> GetPokemons(int top = 100)
        {
            var pokemons = await _pokemonService.GetAllPokemonsAsync(top);

            return Ok(pokemons);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var pokemon = await _pokemonService.GetPokemonByIdAsync(id);

            return pokemon is null ? NotFound() : (IActionResult)Ok(pokemon);
        }

        [HttpGet("[action]/{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            var pokemon = await _pokemonService.GetPokemonByNameAsync(name);

            return pokemon is null ? NotFound() : (IActionResult)Ok(pokemon);
        }
        [HttpGet("[action]/{queryText}")]
        public async Task<IActionResult> FindByString([FromRoute] string queryText)
        {
            var pokemon = await _pokemonService.FindByStringAsync(queryText);

            return pokemon is null ? NotFound() : (IActionResult)Ok(pokemon);
        }
    }
}
