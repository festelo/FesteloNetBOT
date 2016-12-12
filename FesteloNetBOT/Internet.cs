using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FesteloNetBOT
{
    public static class Internet
    {
        public static async Task<string> GetDataAsync(string url, string cookie)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.CookieContainer.Add(new Uri(url), new Cookie("express.sid", cookie));

            var responseTask = webRequest.GetResponseAsync();

            string ret = "";

            using (var response = await responseTask)
            {
                using (StreamReader stream = new StreamReader(
                        response.GetResponseStream(), Encoding.UTF8))
                {
                    ret = stream.ReadToEnd();
                }
            }

            return ret;
        }
        public static class Parse
        {

        }
    }

}
