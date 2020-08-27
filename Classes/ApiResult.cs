namespace PCGWMetaData.Classes
{
    using Newtonsoft.Json;

    public partial class ApiResult
    {
        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public Query Query { get; set; }
    }

    public partial class Query
    {
        [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Datum[] Data { get; set; }

        [JsonProperty("sobj", NullValueHandling = NullValueHandling.Ignore)]
        public Sobj[] Sobj { get; set; }

        [JsonProperty("serializer", NullValueHandling = NullValueHandling.Ignore)]
        public string Serializer { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public double? Version { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("property", NullValueHandling = NullValueHandling.Ignore)]
        public string Property { get; set; }

        [JsonProperty("dataitem", NullValueHandling = NullValueHandling.Ignore)]
        public Dataitem[] Dataitem { get; set; }
    }

    public partial class Dataitem
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public long? Type { get; set; }

        [JsonProperty("item", NullValueHandling = NullValueHandling.Ignore)]
        public string Item { get; set; }
    }

    public partial class Sobj
    {
        [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Datum[] Data { get; set; }
    }
}