using System.Collections.Generic;

namespace JasonConverter
{
    public class Sekcija
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Vsebina> Vsebine { get; set; } = new List<Vsebina>();
    }

}

