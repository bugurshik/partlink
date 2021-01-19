using HtmlAgilityPack;
using ParsingLib;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Partslink24Models;
using System;

namespace ConsoleParsingPartsLinks24
{
    public class ParsingProgram
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
            ModelConfig baseModelConf = new ModelConfig { Model = "Audi A8", Year = "1998", Restrict1 = "Передний привод" };
            GroupConfig baseGroupConig = new GroupConfig { GroupName = "Дигатель", ModelConfig = baseModelConf };

            parsingDetails(baseGroupConig, "https://www.partslink24.com/vwag/audi_parts/image-board.action?catalogMarket=RDW&episType=152&familyKey=91155&illustrationId=7564255&lang=ru&localMarketOnly=true&maingroup=1&modelYear=1998&ordinalNumber=2&partDetailsMarket=RDW&restriction1=92362&startup=false&mode=K00U0RUXX&upds=1381");

            void parsingM(ModelConfig config, string url)
            {
                var path = Site.GetUriPath(url);
                HtmlDocument htmlDoc = Site.LoadDocument(url);

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
                var path = Site.GetUriPath(href);
                var json = Site.LoadJsonString(href);
                var years = Answers.FromJson(json);
                foreach (var year in years.ContentList)
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
                var path = Site.GetUriPath(href);
                var json = Site.LoadJsonString(href);
                var restricts = Answers.FromJson(json);
                foreach (var restrict in restricts.ContentList)
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
                var path = Site.GetUriPath(href);
                var json = Site.LoadJsonString(href);
                var restricts = Answers.FromJson(json);
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
                var json = Site.LoadJsonString(href);
                var restricts = Answers.FromJson(json);
                foreach (var restrict in restricts.ContentList)
                {
                    config.Restrict3 = restrict.Caption.Trim();


                    if (restrict.JsonUrl != null)
                    {
                        var relUrl = restrict.JsonUrl;
                        var path = Site.GetUriPath(href) + relUrl;
                        parsingR3(config, path);
                    }

                    else
                    {
                        var relUrl = restrict.Url;
                        var path = Site.GetUriPath(href) + relUrl;
                        parsingGroup(config, path);
                    }
                }
            }

            void parsingGroup(ModelConfig modelConfig, string href)
            {
                HtmlDocument htmlDoc = Site.LoadDocument(href);

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
                        //modelConfig.GroupConfig = (newGroupConfig);
                        newGroupConfig.ModelConfig = modelConfig;

                        string url = model.Attributes["url"].Value.Trim();
                        var path = Site.GetUriPath(href) + url;

                        parsingPart(newGroupConfig, path);
                    }
                }
            }
            void parsingPart(GroupConfig groupConfig, string href)
            {
                var htmlDoc = Site.LoadDocument(href);
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
                    var path = Site.GetUriPath(href) + url;

                    parsingDetails(groupConfig, path);
                }
            }
            void parsingDetails(GroupConfig groupConfig, string href)
            {
                var htmlDoc = Site.LoadDocument(href).DocumentNode.InnerHtml;

                string tabjePattern = @",(""positions"":(.*?)),""lang";
                string tabjeJson = "{" + Regex.Match(htmlDoc, tabjePattern, RegexOptions.Singleline).Groups[1].Value + "}";
                DetailConfing test = DetailConfigJson.FromJson(tabjeJson);

                string imagePattern = @"""imageViewerParamsUrl"":""(.*?)""";
                string imageJsonUrl = "https://www.partslink24.com/vwag/audi_parts/" + Regex.Match(htmlDoc, imagePattern).Groups[1].Value.Replace("\\u0026", "&");
                ImageParam imageParam = ImageParam.FromJson(Site.LoadJsonString(imageJsonUrl));

                string imageRequestInfo = "&request=GetImageInfo&ticket=";
                string jsonImageInfoUrl = domain + imageParam.Pathparams.Url + imageRequestInfo + imageParam.Pathparams.Ticket + "&cv=1";
                ImageInfo imageInfo = ImageInfo.FromJson(Site.LoadJsonString(jsonImageInfoUrl));

                string ImageSetting = string.Format(@"&rnd=2771&request=GetImage&format={0}&bbox=0%2C0%2C1248%2C1413&width={1}&height={2}&scalefac=1&ticket=",
                    imageInfo.ImageFormat, imageInfo.ImageWidth, imageInfo.ImageHeight);

                //string ImageSetting = "&rnd=2771&request=GetImage&format=image%2Fpng&bbox=0%2C0%2C1248%2C1413&width=500&height=302&scalefac=1&ticket=";
                string imageUrl = domain + imageParam.Pathparams.Url + ImageSetting + imageParam.Pathparams.Ticket.Trim() + "&cv=1";
                string imagePath = Site.LoadImage(imageUrl, @"C:\Users\Proger2\Desktop\");
            }
        }

        public class ParsingModels : ParsingDoc<ModelConfig, ModelConfig>
        {
            public ParsingModels(ModelConfig config, string url) : base(config, url) { }
            public override List<ModelConfig> GetAll()
            {
                var models = Doc.GetElementbyId("nav-model-container").SelectNodes("//tbody/tr");

                foreach (var model in models)
                {
                    if (model.Attributes["url"] != null && model.Attributes["jsonurl"] != null && !model.HasClass("tc-section-row"))
                    {
                        Values.Add(
                        new ModelConfig
                        {
                            Model = model.InnerText,
                            FamilyKey = Regex.Match(model.Attributes["jsonurl"].Value, @"familyKey=(.*?)&").Groups[1].Value
                        });
                    }
                }
                return Values;
            }
        }
        public class Years : ParsingJson<ModelConfig, ModelConfig>
        {
            public Years(ModelConfig config, string url) : base(config, url) { }
            public override List<ModelConfig> GetAll()
            {
                var years = Answers.FromJson(Json);

                foreach (var year in years.ContentList)
                {
                    ModelConfig newYearConfig = new ModelConfig
                    {
                        Model = ParentConfig.Model,
                        FamilyKey = ParentConfig.FamilyKey,
                        Year = year.Caption
                    };
                    Values.Add(newYearConfig);
                }
                return Values;
            }
        }
        public class RestrictI : ParsingJson<ModelConfig, ModelConfig>
        {
            public RestrictI(ModelConfig config, string url) : base(config, url) { }
            public override List<ModelConfig> GetAll()
            {
                var restricts = Answers.FromJson(Json);

                foreach (var restrict in restricts.ContentList)
                {
                    ModelConfig newModelConfig = new ModelConfig
                    {
                        Model = ParentConfig.Model,
                        FamilyKey = ParentConfig.FamilyKey,
                        Year = ParentConfig.Year,
                        Restrict1 = restrict.Caption.Trim(),
                        RestrictKey = Regex.Match(restrict.Url, @"restriction1=(.*?)&").Groups[1].Value                        
                    };
                    Values.Add(newModelConfig);
                }
                return Values;
            }
        }
        public class Groups : ParsingDoc<ModelConfig, GroupConfig>
        {
            public Groups(ModelConfig config, string url) : base(config, url) { }
            public override List<GroupConfig> GetAll() 
            {
                var groups = Doc.GetElementbyId("nav-maingroup-container").SelectNodes("//tbody/tr");

                foreach (var group in groups)
                {
                    if (group.Attributes["url"] != null && group.Attributes["jsonurl"] != null && !group.HasClass("tc-section-row"))
                    {
                        GroupConfig newGroupConfig = new GroupConfig
                        {
                            ModelConfig = ParentConfig,
                            GroupName = group.InnerText,
                            MainGroup = Regex.Match(group.Attributes["url"].Value, @"maingroup=(.*?)&").Groups[1].Value
                        };
                        Values.Add(newGroupConfig);
                    }
                }
                return Values;
            }
        }
        public class Part : ParsingDoc<GroupConfig, GroupConfig>
        {
            public Part(GroupConfig config, string url) : base(config, url) { }
            public override List<GroupConfig> GetAll()
            {
                var parts = Doc.GetElementbyId("nav-groupIllustration-table").SelectNodes("tbody/tr");
                foreach (var part in parts)
                {
                    var cell = part.SelectNodes("td");
                    GroupConfig newGroupConfig = new GroupConfig
                    {
                        ModelConfig = ParentConfig.ModelConfig,
                        GroupName = ParentConfig.GroupName,
                        MainGroup = ParentConfig.MainGroup,
                        GLGR = cell[0].InnerText,
                        Ilustration = cell[1].InnerText,
                        IlustrationId = Regex.Match(part.Attributes["url"].Value, @"illustrationId=(.*?)&").Groups[1].Value,
                        Name = cell[2].InnerText,
                        Notice = cell[3].InnerText == "&nbsp;" ? null : cell[3].InnerText,
                        Inputs = cell[4].InnerText == "&nbsp;" ? null : cell[4].InnerText,
                    };
                    Values.Add(newGroupConfig);
                }
                return Values;
            }
        }
        public class Detail : ParsingDoc<GroupConfig, DetailConfing>
        {
            public Detail(GroupConfig config, string url) : base(config, url) { }
            public override List<DetailConfing> GetAll()
            {
                string domain = "https://www.partslink24.com/";
                string path = "https://www.partslink24.com/vwag/audi_parts";
                string html = Site.LoadDocument(URL).DocumentNode.InnerHtml;
                //string ImageSetting = "&rnd=2771&request=GetImage&format=image%2Fpng&bbox=0%2C0%2C1248%2C1413&width=500&height=302&scalefac=1&ticket=";

                DetailConfing newDetailConfig = DetailConfigJson.FromJson(getTableJson());

                newDetailConfig.ImageInfo = loadImageInfo();
                newDetailConfig.GroupConfig = ParentConfig;
                Values.Add(newDetailConfig);

                return Values;

                ImageInfo loadImageInfo()
                {
                    ImageParam imageParam = ImageParam.FromJson(getImageParamJson());
                    ImageInfo imageInfo = ImageInfo.FromJson(getImageInfoJson());
                    imageInfo.ImageUrl = MakeImageUrl();
                    return imageInfo;

                    string getImageParamJson()
                    {
                        string imagePattern = @"""imageViewerParamsUrl"":""(.*?)""";
                        string imageJsonUrl = path + "/" + Regex.Match(html, imagePattern).Groups[1].Value.Replace("\\u0026", "&");
                        return Site.LoadJsonString(imageJsonUrl);
                    }
                    string getImageInfoJson()
                    {
                        string jsonImageInfoUrl = domain + imageParam.Pathparams.Url + "&cv=1&request=GetImageInfo&ticket=" + imageParam.Pathparams.Ticket;
                        return Site.LoadJsonString(jsonImageInfoUrl);
                    }
                    string MakeImageUrl()
                    {
                        string ImageSetting = string.Format(@"&rnd=2771&request=GetImage&format={0}&cv=1&bbox=0%2C0%2C1248%2C1413&width={1}&height={2}&scalefac=1&ticket=",
                            imageInfo.ImageFormat, imageInfo.ImageWidth, imageInfo.ImageHeight);

                        return domain + imageParam.Pathparams.Url + ImageSetting + imageParam.Pathparams.Ticket.Trim();
                    }
                }
                string getTableJson()
                {
                    string tabjePattern = @",(""positions"":(.*?)),""lang";
                    return "{" + Regex.Match(html, tabjePattern, RegexOptions.Singleline).Groups[1].Value + "}";
                }
            }
        }
    }
}
