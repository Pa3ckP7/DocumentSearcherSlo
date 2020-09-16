using System;
using System.Collections.Generic;
using System.IO;

namespace Morfer
{
    public class Morf
    {
        private static Dictionary<string, string> Stems = new Dictionary<string, string>();

        public static void Prepare(String morfology, String stopwords) 
        {
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
                        if (!Stems.ContainsKey(arr[0])) Stems[arr[0]] = arr[1];
                    }
                }
            }
        }
        public string Stemfy(string word)
        {
            if (Stems.TryGetValue(word, out var value))
            {
                return value;
            }
            return word;
        }
    }
}
