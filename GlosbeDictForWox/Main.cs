using System.Collections.Generic;
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

            //if there is no result (first item is 'open in browser' which is always added.
            if (wordItems.Length == 1)
            {
                results.Add(new Result()
                {
                    Title = "NONE",
                    SubTitle = "No result.",
                    IcoPath = "Images\\pic.png",
                    Action = e =>
                    {
                        return true;
                    }
                });

                return results;
            }

            //add item into result
            foreach (WordItem item in wordItems)
                results.Add(new Result()
                {
                    Title = item.Phrase,
                    SubTitle = item.Meaning,
                    IcoPath = "Images\\pic.png",
                    Action = e =>
                    {
                        if (item.IsOpenWeb)
                        {
                            //open webbrowser
                            System.Diagnostics.Process.Start(item.Meaning);
                        }
                        else
                        {
                            //copy phrase
                            System.Windows.Forms.Clipboard.SetText(item.Phrase);
                        }
                        return true;
                    }
                });

            return results;
        }
    }
}