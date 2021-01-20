using ParsingLib;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PartslinkModels;
using System;
using System.Linq;
using System.Diagnostics;

namespace ConsoleParsingPartsLinks24
{
	public class ParsingProgram
	{
		static void Main(string[] args)
		{

		}
		public class ParsingModels : ParsingDoc, IParsing
		{
			public ParsingModels(string url) : base(url) { }
			public List<Answer> GetAllModels()
			{
				List<Answer> answers =  new List<Answer>();

				var models = Doc.GetElementbyId("nav-model-container").SelectNodes("//tbody/tr");

				foreach (var model in models)
				{
					if (model.Attributes["url"] != null && model.Attributes["jsonurl"] != null && !model.HasClass("tc-section-row"))
					{
						Answer answer = new Answer
						{
							Value = model.InnerText,
							Key = Regex.Match(model.Attributes["jsonurl"].Value, @"familyKey=(.*?)&").Groups[1].Value
						};
						answers.Add(answer);
					}
				}
				return answers;
			}
		}
		public class ParsingYears : ParsingJson
		{
			public ParsingYears(string url) : base(url) { }
			public List<Answer> GetAllModels()
			{
				if(Json == null)
				{
					return null;
				}

				List<Answer> answers = new List<Answer>();
				var years = DeserializeAnswers.FromJson(Json);

				foreach (var year in years.ContentList)
				{
					Answer answer = new Answer
					{
						Value = year.Caption,
						Key = year.Caption
					};
					answers.Add(answer);
				}
				return answers;
			}
		}
		public class ParsingRestrictI : ParsingJson
		{
			public ParsingRestrictI(string url) : base(url) { }
			public List<Answer> GetAllModels()
			{
				List<Answer> answers = new List<Answer>();
				var restricts = DeserializeAnswers.FromJson(Json);

				foreach (var restrict in restricts.ContentList)
				{
					Answer answer = new Answer
					{
						Value = restrict.Caption.Trim(),
						Key = Regex.Match(restrict.Url, @"restriction1=(.*?)&").Groups[1].Value
					};
					answers.Add(answer);
				}
				return answers;
			}
		}
		public class ParsingGroups : ParsingDoc
		{
			public ParsingGroups(string url) : base(url) { }
			public List<Answer> GetAllModels() 
			{
				List<Answer> answers = new List<Answer>();
				var tesdts = Doc.GetElementbyId("nav-maingroup-table");
				var groups = tesdts.SelectNodes("tbody/tr");

				foreach (var group in groups)
				{
					if (group.Attributes["url"] != null && group.Attributes["jsonurl"] != null && !group.HasClass("tc-section-row"))
					{
						Answer answer = new Answer
						{
							Value = group.InnerText,
							Key = Regex.Match(group.Attributes["url"].Value, @"maingroup=(.*?)&").Groups[1].Value
						};

						answers.Add(answer);
					}
				}
				return answers;
			}
		}
		public class ParsingPart : ParsingDoc
		{
			public ParsingPart(string url) : base(url) { }
			public List<GroupConfig> GetAllModels()
			{
				if (Doc == null)
					return null;

				List<GroupConfig> groupConfigs = new List<GroupConfig>();

				var parts = Doc.GetElementbyId("nav-groupIllustration-table").SelectNodes("tbody/tr");
				foreach (var part in parts)
				{
					var cell = part.SelectNodes("td");
					GroupConfig newGroupConfig = new GroupConfig
					{
						GLGR = cell[0].InnerText,
						Ilustration = cell[1].InnerText,
						IlustrationId = Regex.Match(part.Attributes["url"].Value, @"illustrationId=(.*?)&").Groups[1].Value,
						Name = cell[2].InnerText,
						Notice = cell[3].InnerText == "&nbsp;" ? null : cell[3].InnerText,
						Inputs = cell[4].InnerText == "&nbsp;" ? null : cell[4].InnerText,
					};
					groupConfigs.Add(newGroupConfig);
				}
				return groupConfigs;
			}
		}
		public class ParsingDetail : ParsingDoc
		{
			public ParsingDetail(string url) : base(url) { }
			public List<DetailConfing> GetAllModels()
			{
				List<DetailConfing> detailConfings = new List<DetailConfing>();
				string domain = "https://www.partslink24.com/";
				string path = "https://www.partslink24.com/vwag/audi_parts/";
				string html = Site.LoadDocument(URL).DocumentNode.InnerHtml;

				DetailConfing newDetailConfig = DetailConfing.FromJson(getTableJson());

				newDetailConfig.ImageInfo = loadImageInfo();
				detailConfings.Add(newDetailConfig);

				return detailConfings;

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
