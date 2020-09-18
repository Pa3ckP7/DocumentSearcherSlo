using System.Collections.Generic;

namespace JasonConverter
{
    public class Line
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public JConverter Type { get; set; }
    }

}

