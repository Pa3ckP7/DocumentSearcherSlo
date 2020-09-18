using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JasonConverter
{
    public class JConverter {
        public Predmet _Predmet { get; internal set;}
        public JConverter(string location)
        {
            _Predmet = JsonConvert.DeserializeObject<Predmet>(System.IO.File.ReadAllText(location));
        }
        public class Predmet
        {
            public string Title { get; set; }
            public int? Id { get; set; }
            public List<Sekcija> Sekcije { get; set; } = new List<Sekcija>();
            public List<Vsebina> Vsebine { get; set; } = new List<Vsebina>();
            public List<Tema> Teme { get; set; } = new List<Tema>();
            public bool ImaRazrede { get; set; }
        }
        public class Tema
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public List<Sklop> Sklopi { get; set; } = new List<Sklop>();
            public List<Sekcija> Sekcije { get; set; } = new List<Sekcija>();
            public List<Vsebina> Vsebine { get; set; } = new List<Vsebina>();
        }
        public class Sklop
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public List<Sekcija> Sekcije { get; set; } = new List<Sekcija>();
            public List<Podsklop> Podsklopi { get; set; } = new List<Podsklop>();
            public List<Vsebina> Vsebine { get; set; } = new List<Vsebina>();
        }
        public class Podsklop
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public List<Sekcija> Sekcije { get; set; } = new List<Sekcija>();
            public List<Vsebina> Vsebine { get; set; } = new List<Vsebina>();
        }
        public class Sekcija
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public List<Vsebina> Vsebine { get; set; } = new List<Vsebina>();
        }
        public class Vsebina
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public List<string> Tags { get; set; }
            public List<int> Razredi { get; set; }
            public List<Line> Text { get; set; } = new List<Line>();
            public List<string> Images { get; set; } = new List<string>();
        }
        public class Line
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public List<string> Tags { get; set; } = new List<string>();
            public LineType Type { get; set; }
        }
        public enum LineType
        {
            Normal = 0,
            Heading1 = 1,
            Heading2 = 2,
            Heading3 = 3,
            Heading4 = 4,
            List = 5,
            List1 = 6,
            List2 = 7,
            List3 = 8,
            Image = 9
        }
    }
}

