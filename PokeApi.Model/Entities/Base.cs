namespace PokeApi.Model.Entities
{
    public interface IBase
    {
        public int Id { get; set; }
    }

    public class Base : IBase
    {
        public int Id { get; set; }
    }
}
