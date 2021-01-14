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
            string domain = "https://www.partslink24.com";
            //parsingModels(href);

            /* 
            // parsing all
            ModelConfig baseModelConf = new ModelConfig();
            parsingM(baseModelConf, href);
            */

            // handle parsing 
            ModelConfig baseModelConf = new ModelConfig { Model = "Audi A8", Year = "1998", Restrict1 = "Передний привод"};
            GroupConfig baseGroupConig = new GroupConfig { GroupName = "Дигатель", ModelConfig = baseModelConf };
            
            parsingDetails(baseGroupConig, "https://www.partslink24.com/vwag/audi_parts/image-board.action?catalogMarket=RDW&episType=152&familyKey=91155&illustrationId=7564255&lang=ru&localMarketOnly=true&maingroup=1&modelYear=1998&ordinalNumber=2&partDetailsMarket=RDW&restriction1=92362&startup=false&mode=K00U0RUXX&upds=1381");

            void parsingM( ModelConfig config, string url)
            {
                var path = GetPath(href);
                HtmlDocument htmlDoc = LoadDoc(url);

                var models = htmlDoc.GetElementbyId("nav-model-container").SelectNodes("//tbody/tr");

                foreach (var model in models)
                {
                    if (model.HasClass("tc-section-row"))
                        continue;
                    if (model.Attributes["url"] != null && model.Attributes["jsonurl"] != null)
                    {
                        config.Model = model.InnerText;
                        var yearUrl = model.Attributes["jsonurl"].Value;
                        parsingY(config, path + yearUrl);
                    }
                }
            }
            void parsingY(ModelConfig config, string href)
            {
                var path = GetPath(href);
                var json = GetJsonString(href);
                var years = ModelYearsSerialize.FromJson(json);
                foreach(var year in years.ContentList)
                {
                    config.Year = year.Caption;

                    if (year.JsonUrl != null)
                        parsingR1(config, path + year.JsonUrl);
                    else
                        parsingGroup(config, path + year.Url);
                }
            }
            void parsingR1(ModelConfig config, string href)
            {
                var path = GetPath(href);
                var json = GetJsonString(href);
                var restricts = ModelYearsSerialize.FromJson(json);
                foreach(var restrict in restricts.ContentList)
                {
                    config.Restrict1 = restrict.Caption.Trim();

                    if (restrict.JsonUrl != null)
                        parsingR2(config, path + restrict.JsonUrl);
                    else
                        parsingGroup(config, path + restrict.Url);
                }
            }
            void parsingR2(ModelConfig config, string href)
            {
                var path = GetPath(href);
                var json = GetJsonString(href);
                var restricts = ModelYearsSerialize.FromJson(json);
                foreach (var restrict in restricts.ContentList)
                {
                    config.Restrict1 = restrict.Caption.Trim();

                    if (restrict.JsonUrl != null)
                        parsingR3(config, path + restrict.JsonUrl);
                    else
                        parsingGroup(config, path + restrict.Url);
                }
            }
            void parsingR3(ModelConfig config, string href)
            {
                var json = GetJsonString(href);
                var restricts = ModelYearsSerialize.FromJson(json);
                foreach (var restrict in restricts.ContentList)
                {
                    config.Restrict3 = restrict.Caption.Trim();

                    
                    if (restrict.JsonUrl != null)
                    {
                        var relUrl = restrict.JsonUrl;
                        var path = GetPath(href) + relUrl;
                        parsingR3(config, path);
                    }
                       
                    else
                    {
                        var relUrl = restrict.Url;
                        var path = GetPath(href) + relUrl;
                        parsingGroup(config, path);
                    }
                }
            }

            void parsingGroup(ModelConfig modelConfig, string href)
            {
                HtmlDocument htmlDoc = LoadDoc(href);
                
                var models = htmlDoc.GetElementbyId("nav-maingroup-container").SelectNodes("//tbody/tr");
                foreach (var model in models)
                {
                    if (model.HasClass("tc-section-row"))
                        continue;
                    if (model.Attributes["url"] != null && model.Attributes["jsonurl"] != null)
                    {
                        GroupConfig newGroupConfig = new GroupConfig
                        {
                            GroupName = model.InnerText
                        };
                        modelConfig.Groups.Add(newGroupConfig);
                        newGroupConfig.ModelConfig = modelConfig;

                        string url = model.Attributes["url"].Value.Trim();
                        var path = GetPath(href) + url;

                        parsingPart(newGroupConfig, path);
                    }
                }
            }

            void parsingPart(GroupConfig groupConfig, string href)
            {
                var htmlDoc = LoadDoc(href);
                var models = htmlDoc.GetElementbyId("nav-groupIllustration-table").SelectNodes("tbody/tr");

                foreach (var model in models)
                {
                    var t = model.SelectNodes("td");
                    groupConfig.GLGR = t[0].InnerText;
                    groupConfig.Ilustration = t[1].InnerText;
                    groupConfig.Name = t[2].InnerText;
                    groupConfig.Notice = t[3].InnerText == "&nbsp;" ? null : t[3].InnerText;
                    groupConfig.Inputs = t[4].InnerText == "&nbsp;" ? null : t[4].InnerText;

                    var url = model.Attributes["url"].Value;
                    var path = GetPath(href) + url;

                    parsingDetails(groupConfig, path);
                }
            }
            void parsingDetails(GroupConfig groupConfig, string href)
            {
                var htmlDoc = LoadDoc(href).DocumentNode.InnerHtml;

                string tabjePattern = @",(""positions"":(.*?)),""lang";
                string tabjeJson = "{" + Regex.Match(htmlDoc, tabjePattern, RegexOptions.Singleline).Groups[1].Value + "}";
                DetailConfig test = DetailConfig.FromJson(tabjeJson);

                string imagePattern = @"""imageViewerParamsUrl"":""(.*?)""";
                string imageJson = "https://www.partslink24.com/vwag/audi_parts/" + Regex.Match(htmlDoc, imagePattern).Groups[1].Value.Replace("\\u0026", "&");
                ImageParam imageParam = ImageParam.FromJson(GetJsonString(imageJson));

                string imageRequestInfo = "&request=GetImageInfo&ticket=";
                string jsonImageInfoUrl = domain + imageParam.Pathparams.Url + imageRequestInfo + imageParam.Pathparams.Ticket + "&cv=1";
                ImageInfo imageInfo = ImageInfo.FromJson(GetJsonString(jsonImageInfoUrl));
                
                string ImageSetting = string.Format(@"&rnd=2771&request=GetImage&format={0}&bbox=0%2C0%2C1248%2C1413&width={1}&height={2}&scalefac=1&ticket=", 
                    imageInfo.ImageFormat, imageInfo.ImageWidth, imageInfo.ImageHeight);

                //string ImageSetting = "&rnd=2771&request=GetImage&format=image%2Fpng&bbox=0%2C0%2C1248%2C1413&width=500&height=302&scalefac=1&ticket=";
                string imageUrl = domain + imageParam.Pathparams.Url + ImageSetting + imageParam.Pathparams.Ticket.Trim() + "&cv=1";
                string imagePath = LoadImage(imageUrl, @"C:\Users\Proger2\Desktop\");
            }

            void parsingModels(string url)
            {
                string path = GetPath(url);

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
                string path = GetPath(url);

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


            string LoadImage(string href, string foldetPath)
            {
                string path;
                using (WebClient client = new WebClient())
                {
                    path = string.Format(@"{0}{1}.png", foldetPath, Guid.NewGuid());
                    client.DownloadFile(new Uri(href), path);
                }
                return path;
            }
            int? ToNullableInt(string s)
            {
                int i;
                if (int.TryParse(s, out i)) return i;
                return null;
            }
            string GetJsonString(string URL)
            {
                WebRequest request = WebRequest.Create(URL);
                WebResponse response = request.GetResponse();
                string json;
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
            HtmlDocument LoadDoc(string href)
            {
                var web = new HtmlWeb();
                return web.Load(href);
            }
            string GetPath(string url)
            {
                return Regex.Match(url, @"^(.*)\/").Value;
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
            return ModelYearsSerialize.FromJson(json).ContentList;
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

            var models = htmlDoc.GetElementbyId("nav-bom-table").SelectNodes("tbody/tr");
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
                    PartNo = t[2].InnerText,
                    Description = t[3].InnerText,
                    ModelDescription = t[4].InnerText == "&nbsp;" ? null : t[4].InnerText,
                    Quantity = t[5].InnerText == "&nbsp;" ? null : ToNullableInt(t[5].InnerText),
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
