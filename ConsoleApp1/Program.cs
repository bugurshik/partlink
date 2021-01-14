using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ParsingLib;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Partslink24ModelsLib;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string href = "https://www.partslink24.com/vwag/audi_parts/vehicle.action?mode=K00U0RUXX&lang=ru&startup=true";

            Parsing<ModelConf> main = new Parsing<ModelConf>("main");
            main.Context = new ModelConf("MainConfig");
            Parsing<ModelConf> first = new Parsing<ModelConf>("f");
            Parsing<ModelConf> second = new Parsing<ModelConf>("s");

            main.AddSub(first);
            main.AddSub(second);
            main.AddSubRange(first, second);
        }
    }


    class ModelConf
    {
        string Name;
        public ModelConf (string name)
        {
            Name = name;
        }
    }

    interface ILoadCon
    {
        public void LoadContent();
    }

    class Parsing<T> : ILoadCon
    {
        public T Context;
        string URL;
        public Parsing(string url)
        {
            URL = url;
        }
        void Start()
        {
            LoadContent();
            if(SubsParsing.Count!=0)
            {
                SubsParsing.ForEach( sub => sub.LoadContent());
            }
        }
        public void AddSub(Parsing<T> sub)
        {
            SubsParsing.Add(sub);
            sub.Context = Context;
        }
        public void AddSubRange(params Parsing<T>[] subs)
        {
            subs.ToList().ForEach(sub => AddSub(sub));
        }
        public void LoadContent() { }
        public List<Parsing<T>> SubsParsing { get; set; } = new List<Parsing<T>>();
    }

    class ParsingDoc<T> : Parsing<T>
    {
        public HtmlDocument Doc;
        public ParsingDoc(string url) : base(url)
        {
            Doc = Site.LoadDocument(url);
        }
    }
    class ParsingJson<T> : Parsing<T>
    {
        public string Json;
        public ParsingJson(string url) : base(url)
        {
            Json = Site.LoadJsonString(url);
        }
    }
    class ParsingGeneral<T> : Parsing<T>
    {
        public HtmlDocument Doc;
        public string Json;

        public ParsingGeneral(string url) : base(url)
        {
            Doc = Site.LoadDocument(url);
            Json = Site.LoadJsonString(url);
        }
    }
}
