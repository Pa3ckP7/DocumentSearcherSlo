using System.Collections.Generic;

namespace JasonConverter
{
    public class Podsklop
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Sekcija> Sekcije { get; set; } = new List<Sekcija>();
        public List<Vsebina> Vsebine { get; set; } = new List<Vsebina>();
    }

}

