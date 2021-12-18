using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PokeApi.Model.Entities;
using PokeApi.Services.Models;
using PokeApi.Services.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokeApi.Services.Services
{
    public interface IPokemonService
    {
        Task<IEnumerable<Pokemon>> GetAllPokemonsAsync(int topLimit);
        Task<Pokemon> GetPokemonByIdAsync(int id);
        Task<Pokemon> GetPokemonByNameAsync(string name);
        Task<List<Pokemon>> FindByStringAsync(string queryText);
    }

    public class PokemonService : IPokemonService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings appSettings;

        public PokemonService(HttpClient httpClient, IOptions<AppSettings> options)
        {
            _httpClient = httpClient;
            appSettings = options.Value;
        }

        public async Task<IEnumerable<Pokemon>> GetAllPokemonsAsync(int topLimit)
        {
            string queryString = $"?limit={topLimit}&offset=0";
            var response = await _httpClient.GetAsync(appSettings.GET_POKEMONS_URL + queryString);

            if (response.IsSuccessStatusCode is false)
                return new List<Pokemon>();

            var pokeApiResponse = await DeserializeEntity<PokeApiResponse>(response.Content);

            return pokeApiResponse.Results;
        }

        public async Task<Pokemon> GetPokemonByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(appSettings.GET_POKEMON_BY_ID_URL + id);

            if (response.IsSuccessStatusCode is false)
                return null;

            var pokemon = await DeserializeEntity<Pokemon>(response.Content);

            return pokemon;
        }

        public async Task<Pokemon> GetPokemonByNameAsync(string name)
        {
            var response = await _httpClient.GetAsync(appSettings.GET_POKEMON_BY_NAME_URL + name);

            if (response.IsSuccessStatusCode is false)
                return null;

            var pokemon = await DeserializeEntity<Pokemon>(response.Content);

            return pokemon;
        }

        public async Task<List<Pokemon>> FindByStringAsync(string queryText)
        {
            var result = await GetAllPokemonsAsync(appSettings.TotalPokemons);

            var matches = result.Where(x => x.Name.Contains(queryText)).ToList();

            return matches;
        }

        #region Private Methods

        private async Task<T> DeserializeEntity<T>(HttpContent content)
        {
            string body = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(body);
        }

        #endregion
    }
}
