using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ParsingLib;

namespace ConsoleApp1
{
    public partial class ModelYears
    {
        [JsonProperty("tcLinks")]
        public List<Content> ContentList { get; set; }
    }

    public class Content
    {
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("jsonUrl")]
        public string JsonUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class ModelYears
    {
        public static ModelYears FromJson(string json) => JsonConvert.DeserializeObject<ModelYears>(json);
    }
    public class Json<T>
    {
        public static T FromJson(string json) => JsonConvert.DeserializeObject<T>(json);
    }
    public class Test
    {
        void test()
        {
            Json<ModelYears>.FromJson("");
        }
    }
}
