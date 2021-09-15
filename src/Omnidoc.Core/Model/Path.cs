namespace Omnidoc.Model
{
    public class Path : Shape
    {
        public Path ( string data )
        {
            Data = data;
        }

        public string Data { get; }
    }
}