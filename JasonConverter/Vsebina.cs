using System.Collections.Generic;

namespace JasonConverter
{
    public class Vsebina
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<string> Tags { get; set; }
        public List<int> Razredi { get; set; }
        public List<Line> Text { get; set; } = new List<Line>();
        public List<string> Images { get; set; } = new List<string>();
    }

}

