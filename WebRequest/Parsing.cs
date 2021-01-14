using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

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
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
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
                path = string.Format(@"{1}{0}.png", foldetPath, Guid.NewGuid());
                client.DownloadFile(new Uri("https://www.partslink24.com/" + href), path);
            }
            return path;
        }
    }
    public class  Formatter
    {
        public static int? ToNullableInt(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
    }
}
