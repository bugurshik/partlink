using HtmlAgilityPack;
using System.Collections.Generic;

namespace ParsingLib
{
    public abstract class Parsing<T, U> : IParsing<U>
    {
        public string URL { get; }
        public T ParentConfig;
        public List<U> Values { get; set; } = new List<U>();
        public Parsing(T config, string url)
        {
            URL = url;
            ParentConfig = config;
        }
        public abstract List<U> GetAllModels();
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

    public class ParsingResponce<U>
    {
        public IParsing<U> pars;
        public bool Success { get; set; } = false;
        public string Message { get; set; }
    }

    public interface IParsing<U>
    {
        public List<U> GetAllModels();
    }
}
