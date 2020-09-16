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

namespace Searcher
{
    class Program
    {
        static IndexWriter Writer;
        static MultiPhraseQuery Search;
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
            MassAddIndex(Paths["DocumentsDir"]);
            while (true)
            {
                SearchFor(Console.ReadLine());
                Fetch();
            }
        }
        static void Prepare() 
        {
            var AppLuceneVersion = LuceneVersion.LUCENE_48;
            var indexLocation = Paths["indexDir"];
            var dir = FSDirectory.Open(indexLocation);
            var analyzer = new SloveneAnalyzer(AppLuceneVersion);
            var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
            Writer = new IndexWriter(dir, indexConfig);
        }
        static void AddIndex(string name, string text)
        {
            var source = new
            {
                Name = name,
                Text = text
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
        static void SearchFor(string searchquery)
        {
            string[] queries = searchquery.Split(' ');
            // search with a phrase
            Search = new MultiPhraseQuery();
            foreach (string query in queries)
            {
                Search.Add(new Term("text", query));
            }
        }
        static void Fetch()
        {
            // re-use the writer to get real-time updates
            var searcher = new IndexSearcher(Writer.GetReader(applyAllDeletes: true));
            var hits = searcher.Search(Search, 20 /* top 20 */).ScoreDocs;
            foreach (var hit in hits)
            {
                var foundDoc = searcher.Doc(hit.Doc);
                Console.WriteLine(foundDoc.Get("name"));
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
