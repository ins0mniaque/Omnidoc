namespace Omnidoc.Model
{
    public class Glyphs : Shape
    {
        public Glyphs ( string text )
        {
            Text = text;
        }

        public Font?  Font { get; set; }
        public string Text { get; }
    }
}