using JasonConverter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Morfer
{
    class JsonReader
    {
        private static string text; 
        public string Read(Predmet predmet) 
        {
            text = "";
            ReadPredmet(predmet);
            return text;
        }
        private void ReadPredmet(Predmet predmet) 
        {
            predmet.Sekcije.ForEach(sekcija => ReadSekcija(sekcija));
            predmet.Teme.ForEach(tema => ReadTema(tema));
            predmet.Vsebine.ForEach(vsebina => ReadVsebina(vsebina));
        }
        private void ReadTema(Tema tema) 
        {
            tema.Sekcije.ForEach(sekcija => ReadSekcija(sekcija));
            tema.Sklopi.ForEach(sklop => ReadSklop(sklop));
            tema.Vsebine.ForEach(vsebina => ReadVsebina(vsebina));
        }
        private void ReadSklop(Sklop sklop) 
        {
            sklop.Podsklopi.ForEach(podsklop => ReadPodSklop(podsklop));
            sklop.Sekcije.ForEach(sekcija => ReadSekcija(sekcija));
            sklop.Vsebine.ForEach(vsebina => ReadVsebina(vsebina));
        }
        private void ReadPodSklop(Podsklop podsklop) 
        {
            podsklop.Sekcije.ForEach(sekcija => ReadSekcija(sekcija));
            podsklop.Vsebine.ForEach(vsebina => ReadVsebina(vsebina));
        }
        private void ReadSekcija(Sekcija sekcija) 
        {
            sekcija.Vsebine.ForEach(vsebina => ReadVsebina(vsebina));
        }
        private void ReadVsebina(Vsebina vsebina)
        {
            vsebina.Text.ForEach(line =>
            {
                string str = line.Text.Trim();
                if (str.Length > 0) 
                {
                    text += str.ToLower();
                }
            });
        }
    }
}
