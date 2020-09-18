using System.Collections.Generic;

namespace JasonConverter
{
    public class Predmet
    {
        public string Title { get; set; }
        public int? Id { get; set; }
        public List<Sekcija> Sekcije { get; set; } = new List<Sekcija>();
        public List<Vsebina> Vsebine { get; set; } = new List<Vsebina>();
        public List<Tema> Teme { get; set; } = new List<Tema>();
        public bool ImaRazrede { get; set; }
    }

}

