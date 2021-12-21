using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PokeApi.Model.Entities;
using PokeApi.Services.Models;
using PokeApi.Services.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Services.Services
{
    public interface IPokemonService
    {
        Task<IEnumerable<Pokemon>> GetAllPokemonsAsync(int topLimit);
        Task<Pokemon> GetPokemonByIdAsync(int id);
        Task<Pokemon> GetPokemonByNameAsync(string name);
        Task<List<Pokemon>> FindByStringAsync(string queryText);
        Task<byte[]> DownloadByIdAsync(int id);
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

            var pokemonList = await FetchPokemonsData(pokeApiResponse);

            return pokemonList;
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

        public async Task<byte[]> DownloadByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(appSettings.GET_POKEMON_BY_ID_URL + id);

            if (response.IsSuccessStatusCode is false)
                return null;

            string body = await response.Content.ReadAsStringAsync();
            var bytes = Encoding.ASCII.GetBytes(body);

            return bytes;
        }

        #region Private Methods

        private async Task<T> DeserializeEntity<T>(HttpContent content)
        {
            string body = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(body);
        }

        private async Task<List<Pokemon>> FetchPokemonsData(PokeApiResponse pokeApiResponse)
        {
            if (pokeApiResponse?.Results.Any() == false)
                return new List<Pokemon>();

            var list = new List<Pokemon>();

            foreach (var entity in pokeApiResponse.Results)
            {
                var response = await _httpClient.GetAsync(entity.Url);
                var pokemon = await DeserializeEntity<Pokemon>(response.Content);
                list.Add(pokemon);
            }

            return list;
        }

        #endregion
    }
}
