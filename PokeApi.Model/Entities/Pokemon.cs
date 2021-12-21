namespace PokeApi.Model.Entities
{
    public class Pokemon : Base
    {
        public Abilities[] Abilities { get; set; }
        public int Base_Experience { get; set; }
        public Form[] Forms { get; set; }
        public Game_Indices[] Game_Indices { get; set; }
        public int Height { get; set; }
        public Held_Items[] Held_Items { get; set; }
        public bool Is_Default { get; set; }
        public string Location_Area_Encounters { get; set; }
        public Moves[] Moves { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public object[] Past_Types { get; set; }
        public Species Species { get; set; }
        public Sprites Sprites { get; set; }
        public Stats[] Stats { get; set; }
        public Types[] Types { get; set; }
        public int Weight { get; set; }
    }
}
