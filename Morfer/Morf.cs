using JasonConverter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Morfer
{
    public class Morf
    {
        private static Dictionary<string, string> Stems = new Dictionary<string, string>();

        public static void Prepare(String morfology, String stopwords, String docdir) 
        {
            string[] filenames = Directory.GetFiles(docdir, "*.json", SearchOption.AllDirectories);
            HashSet<string> words= new HashSet<string>();
            foreach (string file in filenames) 
            {
                JsonReader jr= new JsonReader();
                string text=jr.Read(JsonConvert.DeserializeObject<Predmet>(File.ReadAllText(file)));
                Regex.Replace(text, @"[\W ]", "");
                Regex.Replace(text, @"[\d]", "");
                string[] textw=text.Split(' ');
                foreach (string word in textw) 
                {
                    if (!words.Contains(word)) words.Add(word); else continue;
                }
            }
            //preapares the Dictionary that will be used to stemfy words
            using (var mf = File.OpenRead(morfology))
            using (var msr = new StreamReader(mf)) 
            {
                while (!msr.EndOfStream)
                {
                    var l = msr.ReadLine();
                    var arr = l.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (!arr[2].StartsWith("K"))
                    {
                        if (!Stems.ContainsKey(arr[0])&& words.Contains(arr[0])) Stems[arr[0]] = arr[1];
                    }
                }
            }
        }
        public static string Stemify(string word)
        {
            if (Stems.TryGetValue(word, out var value))
            {
                return value;
            }
            return word;
        }
    }
}
