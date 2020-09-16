using Morfer;

namespace LuceneSlo
{
    internal class SloveneLematizer
    {
        public char[] ResultBuffer { get; internal set; }
        public int ResultLength { get; internal set; }
        public bool Stem(char[] buffer, int length) 
        {
            var str = new string(buffer, 0, length);
            var result = Morf.Stemify(str);
            ResultBuffer = result.ToCharArray();
            ResultLength = result.Length;
            return true;
        }
    }
}