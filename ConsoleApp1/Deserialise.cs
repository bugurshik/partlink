using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ParsingLib;

namespace ConsoleApp1
{
    public partial class Serialize1
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

    public partial class Serialize1
    {
        public static Serialize1 FromJson(string json) => JsonConvert.DeserializeObject<Serialize1>(json);
    }
    public class Json<T>
    {
        public static T FromJson(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}
