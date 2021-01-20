using Newtonsoft.Json;
using System.Collections.Generic;

namespace PartslinkModels
{
    public class DeserializeAnswers
    {
        [JsonProperty("tcLinks")]
        public List<FirstPageAnswer> ContentList { get; set; }
        public static DeserializeAnswers FromJson(string json) => JsonConvert.DeserializeObject<DeserializeAnswers>(json);
    }

    //image 
    public class ImageParam
    {
        [JsonProperty("pathparams")]
        public Pathparams Pathparams { get; set; }
        public static ImageParam FromJson(string json) => JsonConvert.DeserializeObject<ImageParam>(json);
    }

    //partial
    public partial class DetailConfing
    {
        public static DetailConfing FromJson(string json) => JsonConvert.DeserializeObject<DetailConfing>(json);
    }
}
