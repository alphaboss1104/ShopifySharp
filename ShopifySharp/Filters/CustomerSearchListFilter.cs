using Newtonsoft.Json;

namespace ShopifySharp.Filters
{
    public class CustomerSearchListFilter : ListFilter<Customer>
    {
        /// <summary>
        /// Set the field and direction by which to order results, e.g. "last_order_date DESC".
        /// </summary>
        [JsonProperty("order")]
        public string Order { get; set; }
        
        /// <summary>
        /// Text to search for in the shop's customer data.
        /// </summary>
        [JsonProperty("query")]
        public string Query { get; set; }
    }
}