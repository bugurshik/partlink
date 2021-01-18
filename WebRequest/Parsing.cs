using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ParsingLib
{
    public abstract class Parsing<T, U>
    {
        public string URL;
        public T ParentConfig;
        public U ChildConfig;
        public List<U> Values { get; set; } = new List<U>();
        public Parsing(T config, string url)
        {
            URL = url;
            ParentConfig = config;
        }
        public abstract List<U> GetAll();
        
    }
    public abstract class ParsingDoc<T, U> : Parsing<T, U>
    {
        public HtmlDocument Doc;
        public ParsingDoc(T config, string url) : base(config, url)
        {
            Doc = Site.LoadDocument(URL);
        }
    }
    public abstract class ParsingJson<T, U> : Parsing<T, U>
    {
        public string Json;
        public ParsingJson(T config, string url) : base(config, url)
        {
            Json = Site.LoadJsonString(URL);
        }
    }
    

    public class Content
    {
        public string Caption { get; set; }
        public string JsonUrl { get; set; }
        public string Url { get; set; }
    }


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
    public class Formatter
    {
        public static int? ToNullableInt(string s)
        {
            if (int.TryParse(s, out int i)) return i;
            return null;
        }
    }
}
