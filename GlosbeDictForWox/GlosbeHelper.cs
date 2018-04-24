using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GlosbeDictForWox
{
    class GlosbeHelper
    {
        private const string QUERY = "https://glosbe.com/gapi/translate?from={0}&dest={1}&format=json&phrase={2}";
        private Main.DelOnSearched delOnSearched;
        public GlosbeHelper(Main.DelOnSearched delOnSearched)
        {
            this.delOnSearched = delOnSearched;
        }

        /// <summary>
        /// Search from Glosbe
        /// </summary>
        /// <param name="from">language form</param>
        /// <param name="to">language to</param>
        /// <param name="q">word to find</param>
        public WordItem[] search(string from, string to, string q)
        {
            //create query
            var query = string.Format(QUERY, from, to, q);

            //create request
            var req = WebRequest.Create(query) as HttpWebRequest;
            req.TransferEncoding = "UTF-8";
            req.Method = "GET";

            //get json result0
            string json = "";
            using (var res =  req.GetResponse() as HttpWebResponse)
            {
                using (var resStream = res.GetResponseStream())
                {
                    var jsonStreamReader = new StreamReader(resStream, Encoding.UTF8);
                    json =  jsonStreamReader.ReadToEnd();
                }
            }

            //parse json
            List<WordItem> wordItemList = new List<WordItem>();
            JObject jsonTotal = JObject.Parse(json);

            if ((jsonTotal["result"] as JProperty).Value.ToString().Equals("ok"))
            {
                JToken tuc = jsonTotal["tuc"];
                foreach (JProperty prop in tuc)
                {
                    try
                    {
                        string phrase = "", meaning = "", explain = "";
                        switch (prop.Name)
                        {
                            case "phrase":
                                var token = prop.Value;
                                phrase = token["text"].ToString();
                                break;
                            case "meanings":
                                var arr = prop.Value as JArray;
                                meaning = arr[0]["text"].ToString();
                                explain = arr[1]["text"].ToString();
                                break;
                        }

                        WordItem item = new WordItem(phrase, meaning, explain);
                        wordItemList.Add(item);
                    } catch (Exception e)
                    {
                        Console.WriteLine("Something wrong while getting a word : " + e.Message);
                    }
                }
            }

            return wordItemList.ToArray();
            //callback
            //delOnSearched?.Invoke(wordItemList.ToArray());
        }
    }
}
