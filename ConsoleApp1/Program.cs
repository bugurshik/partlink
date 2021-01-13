using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ParsingLib;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Partslink24ModelsLib;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string href = "https://www.partslink24.com/vwag/audi_parts/vehicle.action?mode=K00U0RUXX&lang=ru&startup=true";

            parsingModels(href);
            
            void parsingModels(string url)
            {
                string path = Regex.Match(url, @"^(.*)\/").Value;

                Parsing parseModels = new Parsing();
                LoadDocModels DocModels = new LoadDocModels(url);
                parseModels.LoadContent(DocModels);

                // models
                foreach (var model in parseModels.Content)
                {
                    Console.WriteLine(model.Caption);

                    if (model.JsonUrl == null)
                        continue;

                    parsingYears(path + model.JsonUrl);
                }
                void parsingYears(string url)
                {
                    Parsing parseYears = new Parsing();
                    LoadJsonYear jsonYear = new LoadJsonYear(url);
                    parseYears.LoadContent(jsonYear);

                    //years
                    foreach (var year in parseYears.Content)
                    {
                        Console.WriteLine("   " + year.Caption);
                        if (year.JsonUrl == null)
                        {
                            parsingGroups(path + year.Url);
                            continue;
                        }
                        parsingRestrictI(path + year.JsonUrl);
                    }
                }
                void parsingRestrictI(string url)
                {
                    Parsing parseRestrict = new Parsing();
                    LoadJsonYear jsonRestrict = new LoadJsonYear(url);
                    parseRestrict.LoadContent(jsonRestrict);

                    // restrict I
                    foreach (var restrict in parseRestrict.Content)
                    {
                        Console.WriteLine("      " + restrict.Caption);
                        if (restrict.JsonUrl == null)
                        {
                            parsingGroups(path + restrict.Url);
                            continue;
                        }

                        parsingRestrictII(path + restrict.Url);
                    }
                }
                void parsingRestrictII(string url)
                {
                    Parsing parseRestrictI = new Parsing();
                    LoadJsonYear jsonRestrictI = new LoadJsonYear(url);
                    parseRestrictI.LoadContent(jsonRestrictI);

                    // restrict II
                    foreach (var restrictI in parseRestrictI.Content)
                    {
                        Console.WriteLine("         " + restrictI.Caption.Trim());
                        if (restrictI.JsonUrl == null)
                        {
                            parsingGroups(path + restrictI.Url);
                            continue;
                        }
                    }
                }
            }




            void parsingGroups (string url)
            {
                string path = Regex.Match(url, @"^(.*)\/").Value;

                Parsing parseGroups = new Parsing();
                LoadDocGroups DocGroups = new LoadDocGroups(url);
                parseGroups.LoadContent(DocGroups);
                foreach(var group in parseGroups.Content)
                {
                    Console.WriteLine("            " + group.Caption);

                    Parsing parseGroupParts = new Parsing();
                    LoadDocGroupParts docGroupParts = new LoadDocGroupParts(path + group.Url);
                    parseGroupParts.LoadContent(docGroupParts);

                    foreach(var part in parseGroupParts.Content)
                    {
                        Console.WriteLine("            " + part.Caption);

                        Parsing parseDetails = new Parsing();
                        LoadDocGroupParts docDetails = new LoadDocGroupParts(path + part.Url);
                        parseDetails.LoadContent(docDetails);
                    }
                }
            }
        }
    }

    class Parsing
    {
        public List<Content> Content;
        public void LoadContent(ILoadContent content)
        {
            Content = content.LoadContent();
        }
    }
    interface ILoadContent
    {
        List<Content> LoadContent();
    }
    class BaseLoad : ILoadContent
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
    class BaseLoadJson : BaseLoad
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
    class BaseLoadDoc : BaseLoad
    {
        public HtmlDocument htmlDoc;
        public List<Content> contentList;
        public BaseLoadDoc(string url) : base(url) 
        {
            htmlDoc = LoadDoc(url);
            contentList = new List<Content>();
        }
        public HtmlDocument LoadDoc(string href)
        {
            var web = new HtmlWeb();
            return web.Load(href);
        }
    }





    class LoadJsonYear : BaseLoadJson
    {
        public LoadJsonYear(string url) : base(url) { }

        public override List<Content> LoadContent()
        {
            string json = GetJsonString(URL);
            return ModelYears.FromJson(json).ContentList;
        }
    }
    class LoadDocModels : BaseLoadDoc
    {
        public LoadDocModels(string url) : base(url) { }
        public override List<Content> LoadContent()
        {
            HtmlDocument htmlDoc = LoadDoc(URL);
            List<Content> contentList = new List<Content>();

            var models = htmlDoc.GetElementbyId("nav-model-container").SelectNodes("//tbody/tr");

            foreach (var model in models)
            {
                if (model.HasClass("tc-section-row"))
                    continue;
                if (model.Attributes["url"] != null && model.Attributes["jsonurl"] != null)
                {
                    Content content = new Content
                    {
                        Url = model.Attributes["url"].Value,
                        Caption = model.InnerText,
                        JsonUrl = model.Attributes["jsonurl"].Value
                    };
                    contentList.Add(content);
                }
            }
            return contentList;
        }
    }
    class LoadDocGroups : BaseLoadDoc
    {
        public LoadDocGroups(string url) : base(url) { }
        public override List<Content> LoadContent()
        {
            HtmlDocument htmlDoc = LoadDoc(URL);
            List<Content> contentList = new List<Content>();

            var models = htmlDoc.GetElementbyId("nav-maingroup-container").SelectNodes("//tbody/tr"); ;
            foreach (var model in models)
            {
                if (model.HasClass("tc-section-row"))
                    continue;
                if (model.Attributes["url"] != null && model.Attributes["jsonurl"] != null)
                {
                    Content content = new Content
                    {
                        Url = model.Attributes["url"].Value,
                        Caption = model.InnerText,
                    };
                    contentList.Add(content);
                }
            }
            return contentList;
        }
    }
    class LoadDocGroupParts : BaseLoadDoc
    {
        public LoadDocGroupParts(string url) : base(url) { }
        public override List<Content> LoadContent()
        {
            var models = htmlDoc.GetElementbyId("nav-groupIllustration-table").SelectNodes("tbody/tr"); ;
            foreach (var model in models)
            {
                Content content = new Content
                {
                    Url = model.Attributes["url"].Value,
                    Caption = "   Many details"
                };
                var t = model.SelectNodes("td");
                Part part = new Part 
                {
                    GLGR = t[0].InnerText,
                    Ilustration = t[1].InnerText,
                    Name = t[2].InnerText,
                    Notice = t[3].InnerText == "&nbsp;" ? null : t[3].InnerText,
                    Inputs = t[4].InnerText == "&nbsp;" ? null : t[4].InnerText,
                };
                contentList.Add(content);
            }
            return contentList;
        }
    }
    class LoadDocDetails : BaseLoadDoc
    {
        public LoadDocDetails(string url) : base(url) { }
        public override List<Content> LoadContent()
        {
            var imgRef = htmlDoc.GetElementbyId("MainFrame").Attributes["src"].Value;

            LoadImage(imgRef, @"C:\Users\Proger2\Desktop");

            var models = htmlDoc.GetElementbyId("nav-groupIllustration-table").SelectNodes("tbody/tr");
            foreach (var model in models)
            {
                Content content = new Content
                {
                    Url = model.Attributes["url"].Value,
                    Caption = "   Many details"
                };
                var t = model.SelectNodes("td");

                Detail detail = new Detail
                {
                    Position = t[1].InnerText,
                    Number = t[2].InnerText,
                    Name = t[3].InnerText,
                    Notice = t[4].InnerText == "&nbsp;" ? null : t[4].InnerText,
                    Count = t[5].InnerText == "&nbsp;" ? null : ToNullableInt(t[5].InnerText),
                    Input = t[6].InnerText == "&nbsp;" ? null : t[6].InnerText,
                    //DetailPack = pack,
                };

                contentList.Add(content);
            }

            return contentList;
            string LoadImage(string href,string foldetPath)
            {
                string path;
                using (WebClient client = new WebClient())
                {
                    path = string.Format(@"{1}{0}.png", foldetPath, Guid.NewGuid());
                    client.DownloadFile(new Uri("https://www.partslink24.com/" + href), path);
                }
                return path;
            }

            int? ToNullableInt(string s)
            {
                int i;
                if (int.TryParse(s, out i)) return i;
                return null;
            }
        }
    }
}
