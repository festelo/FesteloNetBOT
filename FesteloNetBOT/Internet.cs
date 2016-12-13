using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FesteloNetBOT
{
    public static class Internet
    {
        public static class URLs
        {
            public static string CSGO500 = @"http://csgo500.com/";
            public static string CSGO500Reward = @"http://csgo500.com/reward/";
            public static string CSGO500Transfer = @"http://csgo500.com/transfer/";
        }

        public static string GetData(string url, string cookie)
        {
            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("User-agent", "Mozilla/5.0");
                cookieContainer.Add(baseAddress, new Cookie("express.sid", cookie));
                var task = client.GetStringAsync(baseAddress);
                task.Wait();
                return task.Result;
            }
        }

        /// <summary>
        /// Send POST request and get StatusCode
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Cookie"></param>
        /// <param name="CSRFtoken"></param>
        /// <param name="AdditionallyData"></param>
        /// <returns></returns>
        public static int SendData(string url, string cookie, string csrf, Dictionary<string, string> addData = null)
        {
            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler))
            {
                var values = new Dictionary<string, string>
                {
                    { "_csrf", "csrf" }
                };
                if (addData != null)
                    foreach (var s in addData)
                        values.Add(s.Key, s.Value);
                var content = new FormUrlEncodedContent(values);

                client.DefaultRequestHeaders.Add("User-agent", "Mozilla/5.0");
                cookieContainer.Add(baseAddress, new Cookie("express.sid", cookie));
                var task = client.PostAsync(baseAddress, content);
                task.Wait();
                return (int)task.Result.StatusCode;
            }
        }

        public static class Parse
        {
            public static string CSRF(string source)
            {
                Match match = Regex.Match(source, "csrfToken = \"(.+)\";");
                if (match.Success)
                    return match.Groups[1].Value;
                else
                    return null;
            }

            /// <summary>
            /// Get user data from HTML. WITHOUT COOKIE!
            /// </summary>
            public static User User(string source)
            {
                Dictionary<string, string> regexStr = new Dictionary<string, string>
                {
                    { "name", "<div id=\"account-username\">\n(.+?)\n</div>" },
                    { "balance", "value = (\\d+?);" },
                    { "time", "rewardDate = \"(.+?) GMT" }
                };
                Dictionary<string, string> parsedStr = new Dictionary<string, string>();
                foreach (var s in regexStr)
                {
                    Match match = Regex.Match(source, s.Value);
                    if (match.Success)
                        parsedStr.Add(s.Key, match.Groups[1].Value);
                    else return null;
                }

                return new User()
                {
                    Name = parsedStr["name"],
                    Balance = Convert.ToInt32(parsedStr["balance"]),
                    Time = Convert.ToDateTime(parsedStr["time"]).AddDays(1)
                };
            }
        }
    }

}
