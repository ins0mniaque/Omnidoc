namespace Omnidoc
{
    public class ParserOptions
    {
        public Levels Levels { get; set; } = Levels.All;
        public bool   Strict { get; set; }
    }
}