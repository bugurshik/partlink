using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ParsingLib;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string href = "https://www.partslink24.com/vwag/audi_parts/vehicle.action?mode=K00U0RUXX&lang=ru&startup=true";
            string jsonhref = "https://www.partslink24.com/vwag/audi_parts/json-model-years.action?familyKey=91012&lang=ru&localMarketOnly=true&ordinalNumber=2&startup=false&mode=K00U0DEXX&upds=1381";
            /*            Parsing<ModelConf> main = new Parsing<ModelConf>("main");
                        main.Context = new ModelConf("MainConfig");
                        Parsing<ModelConf> first = new Parsing<ModelConf>("f");
                        Parsing<ModelConf> second = new Parsing<ModelConf>("s");

                        Parsing<GroupConf> group = new Parsing<GroupConf>("group");

                        main.AddSub(first);
                        main.AddSub(second);
                        main.AddSubRange(first, second);*/
            Test test = new Test();
            ParsingModels models = new ParsingModels(new ModelConfig(), href);
            // models.GetConfigList().ForEach(model => Console.WriteLine(model.Model));
            ParsingYears years = new ParsingYears(models.GetConfigList()[0], jsonhref);
           // years.GetConfigList().ForEach(model => Console.WriteLine(model.Year));
        }
    }


    public class ModelConfig : ICloneable
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Model { get; set; }
        public string FamilyKey { get; set; }
        public string Year { get; set; }
        public string Restrict1 { get; set; }
        public string Restrict2 { get; set; }
        public string Restrict3 { get; set; }
        // relationship
        public GroupConf GroupConfig = new GroupConf();
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public class GroupConf
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Model { get; set; }
        public string FamilyKey { get; set; }
        public ModelConfig ModelConfig { get; set; } = new ModelConfig();
    }
    public class Test : IEnumerable
    {
        string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday",
                         "Friday", "Saturday", "Sunday" };
        public IEnumerator GetEnumerator()
        {
            return days.GetEnumerator();
        }
    }
    abstract class Parsing<T, U> where T : new()
    {
        public string URL;
        public T ParentConfig;
        public U ChildConfig;
        public List<Parsing<T, U>> SubsParsing { get; set; } = new List<Parsing<T, U>>();

        public List<ContentStr<T>> ContentList = new List<ContentStr<T>>();
        public Parsing(T config, string url)
        {
            URL = url;
            ParentConfig = config;
            CollectContent();
        }
        void Start()
        {
            CollectContent();
            if (SubsParsing.Count != 0)
            {
                SubsParsing.ForEach(sub => sub.CollectContent());
            }
        }

        public List<T> GetConfigList()
        {
            return ContentList.Select(c => c.Model).ToList();
        }

        // content
        virtual public void CollectContent() { }
        public void AddContent(T model, string url)
        {
            ContentList.Add(new ContentStr<T> { Model = model, Url = url });
        }     

        // relationships
        public void AddChild(Parsing<T, U> sub)
        {
            SubsParsing.Add(sub);
            sub.ParentConfig = ParentConfig;
        }
        public void AddSubRange(params Parsing<T, U>[] subs)
        {
            subs.ToList().ForEach(sub => AddChild(sub));
        }
    }
    struct ContentStr<T>
    {
        public T Model;
        public string Url;
    }


    class ParsingModels : Parsing<ModelConfig, ModelConfig>
    {
        public ParsingModels(ModelConfig config, string url) : base(config, url) { }
        public override void CollectContent()
        {
            var Resource = Site.LoadDocument(URL);
            var items = Resource.GetElementbyId("nav-model-table").SelectNodes("tbody/tr");
            foreach (var item in items)
            {
                if (item.Attributes["url"] != null && item.Attributes["jsonurl"] != null && !item.HasClass("tc-section-row"))
                {
                    ModelConfig newConfig = (ModelConfig)ParentConfig.Clone();
                    newConfig.FamilyKey = Regex.Match(item.Attributes["jsonurl"].Value, @"familyKey=(.*?)&").Groups[1].Value;
                    var href = item.Attributes["jsonurl"].Value;
                    AddContent(newConfig, href);
                }
            }
        }
    }
    class ParsingYears : Parsing<ModelConfig, ModelConfig>
    {
        public ParsingYears(ModelConfig config, string url) : base(config, url) { }
        public override void CollectContent()
        {
            Console.WriteLine();

            string Resource = Site.LoadJsonString(URL);

            var items = Serialize1.FromJson(Resource).ContentList;
            foreach( var item in items)
            {
                ModelConfig newConf = (ModelConfig)ParentConfig.Clone();
                newConf.Year = item.Caption;

                if (item.JsonUrl != null)
                    AddContent(newConf, item.JsonUrl);
                else
                    AddContent(newConf, item.Url);

                Console.WriteLine(newConf.Year);
            }
        }
    }
    class ParsingRestrict1 : Parsing<ModelConfig, GroupConf>
    {
        public ParsingRestrict1(ModelConfig config, string url) : base(config, url) { }
        public override void CollectContent()
        {
            string Resource = Site.LoadJsonString(URL);
            var items = Serialize1.FromJson(Resource).ContentList;

            foreach (var item in items)
            {
                AddContent(MakeConfig(item), item.Url);
            }
        }
        public ModelConfig MakeConfig(Content item)
        {
            ModelConfig newConf = (ModelConfig)ParentConfig.Clone();
            newConf.Restrict1 = item.Caption.Trim();
            return newConf;
        }
    }
    class ParsingRestrict2 : ParsingRestrict1
    {
        public ParsingRestrict2(ModelConfig config, string url) : base(config, url) { }
        public new ModelConfig MakeConfig(Content item)
        {
            var newConfig = base.MakeConfig(item);
            newConfig.Restrict2 = item.Caption.Trim();
            return newConfig;
        }
    }
    class ParsingGroup : Parsing<GroupConf, GroupConf> 
    {
        ParsingGroup(GroupConf config, string url) : base(config, url) { }
        public override void CollectContent()
        {
            HtmlDocument htmlDoc = Site.LoadDocument(URL);

            var items = htmlDoc.GetElementbyId("nav-maingroup-container").SelectNodes("//tbody/tr");
            foreach (var item in items)
            {
                if (item.HasClass("tc-section-row"))
                    continue;
                if (item.Attributes["url"] != null && item.Attributes["jsonurl"] != null)
                {
                    GroupConf newGroupConfig = new GroupConf
                    {
                        GroupName = item.InnerText
                    };
                    modelConfig.Groups.Add(newGroupConfig);

                    string url = item.Attributes["url"].Value.Trim();
                    var path = Site.GetUriPath(href) + url;

                    AddContent(newGroupConfig, url);
                }
            }
        }
    }
}
