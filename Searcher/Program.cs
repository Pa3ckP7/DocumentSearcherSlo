using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Morfer;
using LuceneSlo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;
using System.IO;
using Lucene.Net.Analysis.Util;
using Lucene.Net.QueryParsers.Classic;
using Newtonsoft.Json;
using JasonConverter;

namespace Searcher
{
    class Program
    {
        static LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
        static IndexWriter Writer;
        static MultiPhraseQuery Search;
        static SloveneAnalyzer _analyzer;
        public static System.Collections.Generic.Dictionary<string, string> Paths = new System.Collections.Generic.Dictionary<string, string>()
        {
            {"stopwords","resources/stopwords.txt"},
            {"morfologija","resources/morfologija.txt"},
            {"indexDir","Index"},
            {"DocumentsDir","documents"}
        };
        static void Main(string[] args)
        {
            Morf.Prepare(Paths["morfologija"], Paths["stopwords"]);
            Prepare();
            AddIndex($"{Paths["DocumentsDir"]}/Gimnazija.Slovenscina.json");
            //MassAddIndex(Paths["DocumentsDir"]);
            while (true)
            {
                var query = SearchFor(Console.ReadLine());
                Fetch(query);
            }
        }
        static void Prepare() 
        {            
            var indexLocation = Paths["indexDir"];
            var dir = FSDirectory.Open(indexLocation);
            String[] stopwords = ReadText(Paths["stopwords"]).Split('\n');
            CharArraySet stopwords2 = new CharArraySet(AppLuceneVersion,0,true);
            foreach (var stopword in stopwords) 
            {
                stopwords2.Add(stopword);
            }
            _analyzer = new SloveneAnalyzer(AppLuceneVersion, stopwords2);
            var indexConfig = new IndexWriterConfig(AppLuceneVersion, _analyzer);
            Writer = new IndexWriter(dir, indexConfig);
        }
        static void AddIndex(string location)
        {
            var predmet = JsonConvert.DeserializeObject<Predmet>(System.IO.File.ReadAllText(location));
            var indexer = new Indexer(Writer);
            indexer.IndexPredmet(predmet);
        
            Writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }
        static void MassAddIndex(string directory)
        {
            string[] filenames = System.IO.Directory.GetFiles(directory,"*",SearchOption.TopDirectoryOnly);
            for (int i = 0; i < filenames.Length; i++) 
            {
                var source = new
                {
                    Name = Path.GetFileName(filenames[i]),
                    Text = ReadText(filenames[i])
                };
                Document doc = new Document
                {
                    new StringField("name",
                        source.Name,
                        Field.Store.YES),
                    new TextField("text",
                        source.Text,
                        Field.Store.YES),
                };
                Writer.AddDocument(doc);
                Writer.Flush(triggerMerge: false, applyAllDeletes: false);
            }
        }   

        static Query SearchFor(string searchquery)
        {
            var qp = new QueryParser(AppLuceneVersion, "text", _analyzer);
            return qp.Parse(searchquery);
        }
        /*
        static Query MultiphaseSearchFor(string searchquery)
        {
            searchquery = Morf.Stemify(searchquery);
            string[] queries = searchquery.Split(' ');
            // search with a phrase
            var mpq = new MultiPhraseQuery();
            foreach (string query in queries)
            {
                Search.Add(new Term("text", query));
            }
            return mpq;
        }*/
        static void Fetch(Query q)
        {
            // re-use the writer to get real-time updates
            var searcher = new IndexSearcher(Writer.GetReader(applyAllDeletes: true));
            var hits = searcher.Search(q, 20 /* top 20 */).ScoreDocs;
            foreach (var hit in hits)
            {
                var foundDoc = searcher.Doc(hit.Doc);
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine($"Predmet: {foundDoc.Get("predmet")}");
                Console.WriteLine($"Tema: {foundDoc.Get("tema")}");
                Console.WriteLine($"Sklop: {foundDoc.Get("sklop")}");
                Console.WriteLine($"Podsklop: {foundDoc.Get("podkslop")}");
                Console.WriteLine($"Naslov: {foundDoc.Get("sekcija")}");
                Console.WriteLine("-------------------------------------------------------------");
            }
            
        }

      
        static string ReadText(string file) 
        {
            string text = "";
            using (var pfs = File.OpenRead(file))
            using (var psr = new StreamReader(pfs)) 
            {
                while (!psr.EndOfStream) 
                {
                    string line = psr.ReadLine().ToLower().Trim();
                    text += line;
                }
            }
            return text;
        }
    }
}
