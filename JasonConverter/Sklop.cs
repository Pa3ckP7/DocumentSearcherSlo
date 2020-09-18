using System.Collections.Generic;

namespace JasonConverter
{
    public class Sklop
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Sekcija> Sekcije { get; set; } = new List<Sekcija>();
        public List<Podsklop> Podsklopi { get; set; } = new List<Podsklop>();
        public List<Vsebina> Vsebine { get; set; } = new List<Vsebina>();
    }

}

