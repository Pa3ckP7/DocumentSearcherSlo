using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Tartarus.Snowball.Ext;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuceneSlo
{
    public sealed class SloveneLemmaFilter : TokenFilter
    {
        private readonly SloveneLematizer stemmer = new SloveneLematizer();
        private readonly ICharTermAttribute termAtt;
        private readonly IKeywordAttribute keywordAttr;

        public SloveneLemmaFilter(TokenStream @in) : base(@in)
        {
            termAtt = AddAttribute<ICharTermAttribute>();
            keywordAttr = AddAttribute<IKeywordAttribute>();
        }

        public override bool IncrementToken()
        {
            if (!m_input.IncrementToken())
            {
                return false;
            }

            if ((!keywordAttr.IsKeyword) && stemmer.Stem(termAtt.Buffer, termAtt.Length))
            {
                termAtt.CopyBuffer(stemmer.ResultBuffer, 0, stemmer.ResultLength);
            }
            return true;
        }
    }
}
