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
            try
            {
                var web = new HtmlWeb();
                return web.Load(url);
            }
            catch
            {
                return null;
            }
        }
        public static string LoadJsonString(string url)
        {
            try
            {
                string json;
                WebResponse response = WebRequest.Create(url).GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    json = new StreamReader(stream).ReadToEnd();
                }
                response.Close();
                return json;
            }
            catch
            {
                return null;
            }
        }
        public static string LoadImage(string href, string foldetPath)
        {
            try
            {
                string path;
                using (WebClient client = new WebClient())
                {
                    path = string.Format(@"{0}{1}.png", foldetPath, Guid.NewGuid());
                    client.DownloadFile(new Uri(href), path);
                }
                return path;
            }
            catch
            {
                return null;
            }
        }
        public static string GetUriPath(string url)
        {
            return Regex.Match(url, @"^(.*)\/").Value;
        }
    }
}
