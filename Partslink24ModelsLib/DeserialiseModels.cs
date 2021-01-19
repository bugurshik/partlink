using Newtonsoft.Json;
using System.Collections.Generic;

namespace PartslinkModels
{
    public class Answers
    {
        [JsonProperty("tcLinks")]
        public List<Answer> ContentList { get; set; }
        public static Answers FromJson(string json) => JsonConvert.DeserializeObject<Answers>(json);
    }
    public class Answer
    {
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("jsonUrl")]
        public string JsonUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    //image 
    public class ImageParam
    {
        [JsonProperty("pathparams")]
        public Pathparams Pathparams { get; set; }
        public static ImageParam FromJson(string json) => JsonConvert.DeserializeObject<ImageParam>(json);
    }

    public partial class Pathparams
    {
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    //partial
    public partial class DetailConfing
    {
        public static DetailConfing FromJson(string json) => JsonConvert.DeserializeObject<DetailConfing>(json);
    }
}
