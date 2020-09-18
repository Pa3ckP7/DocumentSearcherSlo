using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JasonConverter;

namespace Searcher
{
    class Indexer
    {
        public static JConverter.Predmet Predmet(string location) 
        {
            var jason = new JConverter(location);
            return jason._Predmet;
        }
        public static JConverter.Sekcija Sekcija(JConverter.Predmet predmet, int index) 
        {
            return predmet.Sekcije[index];
        }
        public static string Vsebina(JConverter.Sekcija sekcija) 
        {
            string returnvalue = "";
            foreach (var line in sekcija.Vsebine[0].Text) 
            {
                returnvalue += line.Text;
            }
            return returnvalue;
        }
    }
}
