namespace Omnidoc
{
    public class ParserOptions : Options
    {
        public Levels Levels { get; set; } = Levels.All;
        public bool   Strict { get; set; }
    }
}