namespace GlosbeDictForWox
{
    public class WordItem
    {
        private string _phrase;
        private string _meaning;
        private bool _isOpenWeb;
        public string Phrase { get => _phrase; set => _phrase = value; }
        public string Meaning { get => _meaning; set => _meaning = value; }
        public bool IsOpenWeb { get => _isOpenWeb; }

        public WordItem(string phrase, string meaning, bool isOpenWeb = false)
        {
            _phrase = phrase;
            _meaning = meaning;
            _isOpenWeb = isOpenWeb;
        }
    }
}