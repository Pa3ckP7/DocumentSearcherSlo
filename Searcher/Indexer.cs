using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JasonConverter;
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace Searcher
{
    //https://www.jetbrains.com/idea/
    public class Indexer
    {
        IndexWriter _writer;
        public Indexer(IndexWriter writer)
        {
            _writer = writer;
        }
        //public static IndexPredmet(string location, JConverter.Predmet predmet)
        //{
        //    var jason = new JConverter(location);
        //    return jason._Predmet;
        //}

        //public static JConverter.Predmet Predmet(string location) 
        //{
        //    var jason = new JConverter(location);
        //    return jason._Predmet;
        //}
        //public static JConverter.Sekcija Sekcija(JConverter.Predmet predmet, int index) 
        //{
        //    return predmet.Sekcije[index];
        //}
        //public static string Vsebina(JConverter.Sekcija sekcija) 
        //{
        //    string returnvalue = "";
        //    foreach (var line in sekcija.Vsebine[0].Text) 
        //    {
        //        returnvalue += line.Text;
        //    }
        //    return returnvalue;
        //}

        public void IndexPredmet(Predmet predmet)
        {
            foreach(var tema in predmet.Teme)
            {
                IndexTema(tema, predmet);
            }
            //predmet.Teme.ForEach(tema => IndexTema(tema, predmet));
            foreach (var sekcija in predmet.Sekcije)
            {
                IndexSekcija(sekcija, predmet, "","","");
            }
            //predmet.Sekcije.ForEach(sekcija => IndexSekcija(sekcija, predmet));
            foreach (var vsebina in predmet.Vsebine)
            {
                IndexVsebina(vsebina, predmet, "predmet", predmet.Id.GetValueOrDefault(),"","","","");
            }
            //predmet.Vsebine.ForEach(vsebina => IndexVsebina(vsebina, predmet,"predmet",predmet.Id.GetValueOrDefault()));
        }

        private void IndexTema(Tema tema, Predmet predmet)
        {
            foreach (var sklop in tema.Sklopi)
            {
                IndexSklop(sklop, predmet, tema.Title);
            }
            //tema.Sklopi.ForEach(sklop => IndexSklop(sklop, predmet));
            foreach (var sekcija in tema.Sekcije)
            {
                IndexSekcija(sekcija, predmet, tema.Title,"","");
            }
            foreach (var vsebina in tema.Vsebine)
            {
                IndexVsebina(vsebina, predmet, "tema", tema.Id, tema.Title,"","","");
            }
        }

        private void IndexSklop(Sklop sklop, Predmet predmet, string tema)
        {
            foreach (var podsklop in sklop.Podsklopi)
            {
                IndexPodSklop(podsklop, predmet, tema, sklop.Title);
            }
            foreach (var sekcija in sklop.Sekcije)
            {
                IndexSekcija(sekcija, predmet, tema, sklop.Title,"");
            }
            foreach (var vsebina in sklop.Vsebine)
            {
                IndexVsebina(vsebina, predmet, "sklop", sklop.Id, tema, sklop.Title,"","");
            }
        }

        private void IndexPodSklop(Podsklop podsklop, Predmet predmet, string tema, string sklop)
        {                     
            foreach (var sekcija in podsklop.Sekcije)
            {
                IndexSekcija(sekcija, predmet, tema, sklop, podsklop.Title);
            }
            foreach (var vsebina in podsklop.Vsebine)
            {
                IndexVsebina(vsebina, predmet, "podsklop", podsklop.Id, tema, sklop, podsklop.Title,"");
            }
            podsklop.Vsebine.ForEach(vsebina => IndexVsebina(vsebina, predmet, "podsklop", podsklop.Id, tema, sklop, podsklop.Title,""));
        }


        private void IndexVsebina(Vsebina vsebina, Predmet predmet, string tip, int id,string tema, string sklop, string podsklop, string sekcija)
        {
            var text = string.Join("\n", vsebina.Text.Select(line => line.Text));
            var d = new Document();
            d.Add(new TextField("text", text, Field.Store.YES));
            d.Add(new Int32Field("id", vsebina.Id, Field.Store.YES));
            d.Add(new StringField("predmet", predmet.Title, Field.Store.YES));
            d.Add(new StringField("tip", tip, Field.Store.YES));
            d.Add(new Int32Field("parentid", id, Field.Store.YES));
            d.Add(new Int32Field("predmetId", predmet.Id??1, Field.Store.YES));
            d.Add(new StringField("tema", tema, Field.Store.YES));
            d.Add(new StringField("sklop", sklop, Field.Store.YES));
            d.Add(new StringField("podsklop", podsklop, Field.Store.YES));
            d.Add(new StringField("sekcija", sekcija, Field.Store.YES));

            _writer.AddDocument(d);

        }

        private void IndexSekcija(Sekcija sekcija, Predmet predmet, string tema, string sklop, string podsklop)
        {
            foreach (var vsebina in sekcija.Vsebine)
            {
                IndexVsebina(vsebina, predmet, "sekcija", sekcija.Id, tema, sklop, podsklop, sekcija.Title);
            }
        }
    }
}
