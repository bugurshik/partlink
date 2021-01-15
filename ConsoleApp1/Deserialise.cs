using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ParsingLib;

namespace ConsoleParsingPartsLinks24
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

    public partial class ModelYearsSerialize
    {
        public static ModelYears FromJson(string json) => JsonConvert.DeserializeObject<ModelYears>(json);
    }



    public partial class DetailConfig
    {
        [JsonProperty("positions")]
        public List<Position> Positions { get; set; }
    }

    public partial class Position
    {
        [JsonProperty("quantity")]
        public List<string> Quantity { get; set; }

        [JsonProperty("originalPartNumber")]
        public List<string> OriginalPartNumber { get; set; }

        [JsonProperty("link")]
        public object Link { get; set; }

        [JsonProperty("description")]
        public List<string> Description { get; set; }

        [JsonProperty("remark")]
        public List<string> Remark { get; set; }

        [JsonProperty("hotspotIds")]
        [JsonConverter(typeof(DecodeArrayConverter))]
        public List<long> HotspotIds { get; set; }

        [JsonProperty("vehicleDataAvailable")]
        public bool VehicleDataAvailable { get; set; }

        [JsonProperty("positionId")]
        public long PositionId { get; set; }

        [JsonProperty("expandedPartNumber")]
        public string ExpandedPartNumber { get; set; }

        [JsonProperty("modelInfo")]
        public List<string> ModelInfo { get; set; }

        [JsonProperty("header")]
        public bool Header { get; set; }

        [JsonProperty("partNumber")]
        public string PartNumber { get; set; }

        [JsonProperty("position")]
        public string PositionPosition { get; set; }

        [JsonProperty("idx")]
        public long Idx { get; set; }

        [JsonProperty("fiInvalid")]
        public bool FiInvalid { get; set; }

        [JsonProperty("infoButtonAvailable")]
        public bool InfoButtonAvailable { get; set; }

        [JsonProperty("theOneForHotspot")]
        public bool TheOneForHotspot { get; set; }

        [JsonProperty("deploymentTime")]
        public string DeploymentTime { get; set; }
    }

    public partial class DetailConfig
    {
        public static DetailConfig FromJson(string json) => JsonConvert.DeserializeObject<DetailConfig>(json, Converter.Settings);
    }
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class DecodeArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(List<long>);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = new List<long>();
            while (reader.TokenType != JsonToken.EndArray)
            {
                var converter = ParseStringConverter.Singleton;
                var arrayItem = (long)converter.ReadJson(reader, typeof(long), null, serializer);
                value.Add(arrayItem);
                reader.Read();
            }
            return value;
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (List<long>)untypedValue;
            writer.WriteStartArray();
            foreach (var arrayItem in value)
            {
                var converter = ParseStringConverter.Singleton;
                converter.WriteJson(writer, arrayItem, serializer);
            }
            writer.WriteEndArray();
            return;
        }

        public static readonly DecodeArrayConverter Singleton = new DecodeArrayConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }



    // image 
    public partial class ImageParam
    {
        [JsonProperty("pathparams")]
        public Pathparams Pathparams { get; set; }
    }

    public partial class Pathparams
    {
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public partial class ImageParam
    {
        public static ImageParam FromJson(string json) => JsonConvert.DeserializeObject<ImageParam>(json);
    }
    public partial class ImageInfo
    {
        [JsonProperty("imageWidth")]
        public long ImageWidth { get; set; }

        [JsonProperty("imageHeight")]
        public long ImageHeight { get; set; }

        [JsonProperty("imageFormat")]
        public string ImageFormat { get; set; }

        [JsonProperty("maxScaleFactor")]
        public long MaxScaleFactor { get; set; }

        [JsonProperty("hotspots")]
        public List<Hotspot> Hotspots { get; set; }
    }

    public partial class Hotspot
    {
        [JsonProperty("hsKey")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long HsKey { get; set; }

        [JsonProperty("hsPartNo")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long HsPartNo { get; set; }

        [JsonProperty("hsGroup")]
        public List<object> HsGroup { get; set; }

        [JsonProperty("hsX")]
        public long HsX { get; set; }

        [JsonProperty("hsY")]
        public long HsY { get; set; }

        [JsonProperty("hsWidth")]
        public long HsWidth { get; set; }

        [JsonProperty("hsHeight")]
        public long HsHeight { get; set; }
    }

    public partial class ImageInfo
    {
        public static ImageInfo FromJson(string json) => JsonConvert.DeserializeObject<ImageInfo>(json);
    }
}
