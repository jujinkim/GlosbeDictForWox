using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlosbeDictForWox
{
    public class WordItem
    {
        private string _phrase;
        private string _meaning;
        private string _explain;

        public string Phrase { get => _phrase; set => _phrase = value; }
        public string Meaning { get => _meaning; set => _meaning = value; }
        public string Explain { get => _explain; set => _explain = value; }

        public WordItem(string phrase, string meaning, string explain)
        {
            _phrase = phrase;
            _meaning = meaning;
            _explain = explain;
        }
    }
}
