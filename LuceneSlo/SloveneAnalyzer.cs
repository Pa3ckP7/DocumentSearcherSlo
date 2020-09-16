using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LuceneSlo
{
    public sealed class SloveneAnalyzer : StopwordAnalyzerBase
    {        
        public const int DEFAULT_MAX_TOKEN_LENGTH = 255;
        private int maxTokenLength = DEFAULT_MAX_TOKEN_LENGTH;        
        public static CharArraySet STOP_WORDS_SET = new CharArraySet(LuceneVersion.LUCENE_48, 0, true);        
        public SloveneAnalyzer(LuceneVersion matchVersion, CharArraySet stopWords)
            : base(matchVersion, stopWords)
        {
            STOP_WORDS_SET = stopWords;
        }        
        public SloveneAnalyzer(LuceneVersion matchVersion)
            : this(matchVersion, STOP_WORDS_SET)
        {
        }        
        public SloveneAnalyzer(LuceneVersion matchVersion, TextReader stopwords)
            : this(matchVersion, LoadStopwordSet(stopwords, matchVersion))
        {
        }        
        public int MaxTokenLength
        {
            get => maxTokenLength;
            set => maxTokenLength = value;
        }
        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            var src = new StandardTokenizer(m_matchVersion, reader);
            src.MaxTokenLength = maxTokenLength;
            TokenStream tok = new StandardFilter(m_matchVersion, src);
            tok = new LowerCaseFilter(m_matchVersion, tok);
            tok = new StopFilter(m_matchVersion, tok, STOP_WORDS_SET);
            tok = new SloveneLemmaFilter(tok);
            return new TokenStreamComponentsAnonymousInnerClassHelper(this, src, tok);
        }
        private class TokenStreamComponentsAnonymousInnerClassHelper : TokenStreamComponents
        {
            private readonly SloveneAnalyzer outerInstance;

            private readonly StandardTokenizer src;

            public TokenStreamComponentsAnonymousInnerClassHelper(SloveneAnalyzer outerInstance, StandardTokenizer src, TokenStream tok)
                : base(src, tok)
            {
                this.outerInstance = outerInstance;
                this.src = src;
            }

            protected override void SetReader(TextReader reader)
            {
                src.MaxTokenLength = outerInstance.maxTokenLength;
                base.SetReader(reader);
            }
        }
    }
}
