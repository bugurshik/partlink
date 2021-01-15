using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ParsingLib;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Partslink24Models;
using System.Linq;

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

            //var t = GetSum(-2, 3);
            Console.WriteLine();

            int GetSum(int a, int b)
            {
                if(a < 0)
                {
                    var r = Enumerable.Range(a, b - a+1);
                    var t = Enumerable.Range(a, b - a).Sum();
                    return t;
                }
                else
                {
                    return Enumerable.Range(a, a - b).Sum();
                }
            } 


            // handle parsing 
            ModelConfig baseModelConf = new ModelConfig { Model = "Audi A8", Year = "1998", Restrict1 = "Передний привод"};
            GroupConfig baseGroupConig = new GroupConfig { GroupName = "Дигатель", ModelConfig = baseModelConf };
            
            parsingDetails(baseGroupConig, "https://www.partslink24.com/vwag/audi_parts/image-board.action?catalogMarket=RDW&episType=152&familyKey=91155&illustrationId=7564255&lang=ru&localMarketOnly=true&maingroup=1&modelYear=1998&ordinalNumber=2&partDetailsMarket=RDW&restriction1=92362&startup=false&mode=K00U0RUXX&upds=1381");

            void parsingM( ModelConfig config, string url)
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
                var path = Site.GetUriPath(href);
                var json = Site.LoadJsonString(href);
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
                var path = Site.GetUriPath(href);
                var json = Site.LoadJsonString(href);
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
                var json = Site.LoadJsonString(href);
                var restricts = ModelYearsSerialize.FromJson(json);
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
                        modelConfig.Groups.Add(newGroupConfig);
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
                DetailConfig test = DetailConfig.FromJson(tabjeJson);

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
        public class Answer
        {
            public string ModelName;
            public string URL;
        }
        public static class Models
        {
            public static List<ModelConfig> GetAll(string url)
            {
                HtmlDocument htmlDoc = Site.LoadDocument(url);
                List<ModelConfig> answers = new List<ModelConfig>();
                var models = htmlDoc.GetElementbyId("nav-model-container").SelectNodes("//tbody/tr");

                foreach (var model in models)
                {
                    if (model.HasClass("tc-section-row"))
                        continue;

                    if (model.Attributes["url"] != null && model.Attributes["jsonurl"] != null)
                    {
                        answers.Add(
                        new ModelConfig
                        {
                            Model = model.InnerText,
                            FamilyKey = Regex.Match(model.Attributes["jsonurl"].Value, @"familyKey=(.*?)&").Groups[1].Value
                        });
                    }
                }
                return answers;
            }
        }
        public static class Years
        {
            public static List<ModelConfig> GetAll(ModelConfig config, string url)
            {
                var json = Site.LoadJsonString(url);
                var years = ModelYearsSerialize.FromJson(json);
                
                List<ModelConfig> answers = new List<ModelConfig>();

                foreach (var year in years.ContentList)
                {
                    ModelConfig newConf = new ModelConfig
                    {
                        Model = config.Model,
                        FamilyKey = config.FamilyKey,
                        Year = year.Caption
                    };
                    answers.Add(newConf);
                }
                return answers;
            }
        }
        public static class RestrictI
        {
            public static List<ModelConfig> GetAll(ModelConfig config, string url)
            {
                var json = Site.LoadJsonString(url);
                var restricts = ModelYearsSerialize.FromJson(json);

                List<ModelConfig> answers = new List<ModelConfig>();

                foreach (var restrict in restricts.ContentList)
                {
                    ModelConfig newConf = new ModelConfig
                    {
                        Model = config.Model,
                        FamilyKey = config.FamilyKey,
                        Year = config.Year,
                        Restrict1 = restrict.Caption.Trim()
                    };
                    answers.Add(newConf);
                }
                return answers;
            }
        }
        public static class Groups
        {
            public static List<GroupConfig> GetAll(ModelConfig config, string url) 
            {
                List<GroupConfig> answers = new List<GroupConfig>();
                GroupConfig groupConfig = new GroupConfig
                {
                    ModelConfig = config,

                };

                return groupConfig;
            }
        }
    }
}
