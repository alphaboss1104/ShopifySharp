using Newtonsoft.Json;

namespace ShopifySharp.Filters
{
    public class CountryCountFilter : Parameterizable
    {
        /// <summary>
        /// Restrict results to after the specified ID. 
        /// </summary>
        [JsonProperty("since_id")]
        public long? SinceId { get; set; }
    }
}