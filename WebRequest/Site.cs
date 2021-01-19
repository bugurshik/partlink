using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ParsingLib
{
    public class Site
    {
        public static HtmlDocument LoadDocument(string url)
        {
            var web = new HtmlWeb();
            return web.Load(url);
        }
        public static string LoadJsonString(string url)
        {
            WebResponse response = WebRequest.Create(url).GetResponse();
            string json = "";
            using (Stream stream = response.GetResponseStream())
            {
                using StreamReader reader = new StreamReader(stream);
                json = reader.ReadToEnd();
            }
            response.Close();
            return json;
        }
        public static string LoadImage(string href, string foldetPath)
        {
            string path;
            using (WebClient client = new WebClient())
            {
                path = string.Format(@"{0}{1}.png", foldetPath, Guid.NewGuid());
                client.DownloadFile(new Uri(href), path);
            }
            return path;
        }
        public static string GetUriPath(string url)
        {
            return Regex.Match(url, @"^(.*)\/").Value;
        }
    }
}
