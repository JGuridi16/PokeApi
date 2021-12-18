using PokeApi.Model.Entities;

namespace PokeApi.Services.Models
{
    public class PokeApiResponse
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public Pokemon[] Results { get; set; }
    }
}
