using HtmlAgilityPack;
using System;
using System.Collections.Generic;

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

    public class Formatter
    {
        public static int? ToNullableInt(string s)
        {
            if (int.TryParse(s, out int i)) return i;
            return null;
        }
    }
}
