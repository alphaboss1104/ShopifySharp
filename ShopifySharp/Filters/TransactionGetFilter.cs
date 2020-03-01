using Newtonsoft.Json;

namespace ShopifySharp.Filters
{
    public class TransactionGetFilter : Parameterizable
    {
        /// <summary>
        /// Retrieve only certain fields, specified by a comma-separated list of field names. 
        /// </summary>
        [JsonProperty("fields")]
        public string Fields { get; set; }
        
        /// <summary>
        /// Show amounts in the shop currency. 
        /// </summary>
        [JsonProperty("in_shop_currency")]
        public bool? InShopCurrency { get; set; }
    }
}