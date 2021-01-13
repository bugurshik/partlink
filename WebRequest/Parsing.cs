using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ParsingLib
{
    public class Parsing
    {
        public List<Content> Content;
        public void LoadContent(ILoadContent content)
        {
            Content = content.LoadContent();
        }
    }
    public interface ILoadContent
    {
        List<Content> LoadContent();
    }
    public class BaseLoad : ILoadContent
    {
        public string URL;
        public BaseLoad(string url)
        {
            URL = url;
        }
        virtual public List<Content> LoadContent()
        {
            return null;
        }
    }
    public class BaseLoadJson : BaseLoad
    {
        public BaseLoadJson(string url) : base(url) { }
        public string GetJsonString(string URL)
        {
            WebRequest request = WebRequest.Create(URL);
            WebResponse response = request.GetResponse();
            string json = "";
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                }
            }
            response.Close();
            return json;
        }
    }
    public class BaseLoadDoc : BaseLoad
    {
        public BaseLoadDoc(string url) : base(url) { }
        public HtmlDocument LoadDoc(string href)
        {
            var web = new HtmlWeb();
            return web.Load(href);
        }
    }
    public class Content
    {
        public string Caption { get; set; }
        public string JsonUrl { get; set; }
        public string Url { get; set; }
    }
}
