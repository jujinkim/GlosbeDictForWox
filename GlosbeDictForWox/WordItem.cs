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
        public string Phrase { get => _phrase; set => _phrase = value; }
        public string Meaning { get => _meaning; set => _meaning = value; }
        public WordItem(string phrase, string meaning)
        {
            _phrase = phrase;
            _meaning = meaning;
        }
    }
}
