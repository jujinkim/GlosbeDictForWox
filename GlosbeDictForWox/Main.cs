using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wox.Plugin;

namespace GlosbeDictForWox
{
    public class Main : IPlugin
    {
        private GlosbeHelper mGlosbeHelper;
        public delegate void DelOnSearched(WordItem[] searchResult);
        private DelOnSearched delOnSearched;

        public void Init(PluginInitContext context)
        {
            delOnSearched = onSearched;
            mGlosbeHelper = new GlosbeHelper(delOnSearched);
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            string from = query.FirstSearch;
            string dest = query.SecondSearch;
            string word = query.ThirdSearch;

            WordItem[] wordItems = mGlosbeHelper.search(from, dest, word);

            foreach (WordItem item in wordItems)
                results.Add(new Result()
                {
                    Title = item.Meaning,
                    SubTitle = string.Format("[{0}] {1}", item.Phrase, item.Explain)
                });

            return results;
        }

        private void onSearched(WordItem[] searchResult)
        {

        }
    }
}
