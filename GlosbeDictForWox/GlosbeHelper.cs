using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace GlosbeDictForWox
{
    internal class GlosbeHelper
    {
        private const string QUERY = "https://glosbe.com/gapi/translate?from={0}&dest={1}&format=json&phrase={2}";

        public GlosbeHelper()
        {
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
            List<WordItem> list = new List<WordItem>();
            list.Add(new WordItem("", ""));
            //create request
            var req = WebRequest.Create(query) as HttpWebRequest;
            req.Method = "GET";

            //get json result0
            string json = "";
            try
            {
                using (var res = req.GetResponse() as HttpWebResponse)
                {
                    using (var resStream = res.GetResponseStream())
                    {
                        var jsonStreamReader = new StreamReader(resStream, Encoding.UTF8);
                        json = jsonStreamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ee)
            {
                json = ee.Message;
                List<WordItem> wordItemListError = new List<WordItem>();
                wordItemListError.Add(new WordItem("fail", "Fail to get result"));
                return wordItemListError.ToArray();
            }
            //parse json
            List<WordItem> wordItemList = new List<WordItem>();
            JObject jsonTotal = JObject.Parse(json);

            if (jsonTotal["result"].ToString() == "ok")
            {
                //read 'tuc'
                JArray tuc = jsonTotal["tuc"] as JArray;
                //read each result in tuc
                foreach (JToken tok in tuc)
                {
                    //read each information in token
                    foreach (JProperty prop in (tok as JObject).Properties())
                    {
                        try
                        {
                            string phrase = "", meaning = "";
                            switch (prop.Name)
                            {
                                case "phrase":
                                    var token = prop.Value;
                                    phrase = token["text"].ToString();
                                    break;

                                case "meanings":
                                    var arr = prop.Value as JArray;
                                    StringBuilder sb = new StringBuilder();
                                    if (arr != null)
                                    {
                                        for (int i = 0; i < arr.Count; i++)
                                        {
                                            sb.Append(arr[i]["text"].ToString());
                                            sb.Append(" | ");
                                        }
                                        meaning = sb.ToString();
                                    }
                                    break;
                            }

                            //add searched word
                            if (!string.IsNullOrEmpty(phrase))
                            {
                                WordItem item = new WordItem(phrase, meaning);
                                wordItemList.Add(item);
                            }
                        }
                        catch (Exception e)
                        {
                            //or add error
                            WordItem item = new WordItem("fail", "parsing error" + e.Message);
                            wordItemList.Add(item);
                        }
                    }
                }
            }

            return wordItemList.ToArray();
        }
    }
}