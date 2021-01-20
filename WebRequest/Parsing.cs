using HtmlAgilityPack;
using System.Collections.Generic;

namespace ParsingLib
{
    public abstract class Parsing
    {
        public string URL { get; }
        public Parsing(string url)
        {
            URL = url;
        }
    }
    public abstract class ParsingDoc : Parsing
    {
        public HtmlDocument Doc;
        public ParsingDoc(string url) : base(url)
        {
            Doc = Site.LoadDocument(URL);
        }
    }
    public abstract class ParsingJson : Parsing
    {
        public string Json;
        public ParsingJson(string url) : base(url)
        {
            Json = Site.LoadJsonString(URL);
        }
    }
    public class Answer
    {
        public string Value { get; set; }
        public string Key { get; set; }
    }

    public class ParsingResponce
    {
        public IParsing pars;
        public bool Success { get; set; } = false;
        public string Message { get; set; }
    }

    public interface IParsing
    {
        public List<Answer> GetAllModels();
    }
}
