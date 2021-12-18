namespace PokeApi.Model.Entities
{
    public class Pokemon : Base
    {
        public Abilities[] Abilities { get; set; }
        public int BaseExperience { get; set; }
        public Form[] Forms { get; set; }
        public Game_Indices[] GameIndices { get; set; }
        public int Height { get; set; }
        public Held_Items[] HeldItems { get; set; }
        public bool IsDefault { get; set; }
        public string LocationAreaEncounters { get; set; }
        public Moves[] Moves { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public object[] PastTypes { get; set; }
        public Species Species { get; set; }
        public Sprites Sprites { get; set; }
        public Stats[] Stats { get; set; }
        public Types[] Types { get; set; }
        public int Weight { get; set; }
    }
}
