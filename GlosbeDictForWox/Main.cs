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
        public void Init(PluginInitContext context)
        {
            mGlosbeHelper = new GlosbeHelper();
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            string from = query.FirstSearch;
            string dest = query.SecondSearch;
            string word = query.ThirdSearch;

            WordItem[] wordItems = mGlosbeHelper.search(from, dest, word);

            //if there is no result
            if(wordItems.Length == 0)
            {
                results.Add(new Result()
                {
                    Title = "NONE",
                    SubTitle = "No result."
                });

                return results;
            }

            //add item into result
            foreach (WordItem item in wordItems)
                results.Add(new Result()
                {
                    Title = item.Phrase,
                    SubTitle = item.Meaning,
                    IcoPath = "Images\\pic.png"
                });

            return results;
        }

    }
}
