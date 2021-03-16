using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ShopifySharp
{
    public class Refund : ShopifyObject
    {
        /// <summary>
        /// The unique identifier of the order.
        /// </summary>
        [JsonProperty("order_id")]
        public long? OrderId { get; set; }

        /// <summary>
        /// The date and time when the refund was created. 
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        ///<summary>
        /// Whether to send a refund notification to the customer
        /// </summary>
        [JsonProperty("notify")]
        public bool? Notify { get; set; }

        /// <summary>
        /// Specify how much shipping to refund.
        /// </summary>
        [JsonProperty("shipping")]
        public Shipping Shipping { get; set; }

        /// <summary>
        /// The three-letter code (ISO 4217 format) for the currency used for the refund. Note: Required whenever the shipping amount property is provided.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// The list of <see cref="RefundOrderAdjustment"/> objects
        /// </summary>
        [JsonProperty("order_adjustments")]
        public IEnumerable<RefundOrderAdjustment> OrderAdjustments { get; set; }

        /// <summary>
        /// The date and time when the refund was imported.
        /// </summary>
        /// <remarks>
        /// This value can be set to dates in the past when importing from other systems. If no value is provided, it will be auto-generated.
        /// </remarks>
        [JsonProperty("processed_at")]
        public DateTimeOffset? ProcessedAt { get; set; }

        /// <summary>
        /// The optional note attached to a refund.
        /// </summary>
        [JsonProperty("note")]
        public string Note { get; set; }

        /// <summary>
        /// An optional comment that explains a discrepancy between calculated and actual refund amounts. 
        /// Used to populate the reason property of the resulting order adjustment object attached to the refund.
        /// </summary>
        /// <value>restock, damage, customer, and other.</value>
        [JsonProperty("discrepancy_reason")]
        public string DiscrepancyReason { get; set; }

        /// <summary>
        /// The list of <see cref="RefundLineItem"/> objects
        /// </summary>
        [JsonProperty("refund_line_items")]
        public IEnumerable<RefundLineItem> RefundLineItems { get; set; }

        /// <summary>
        /// The list of <see cref="Transaction"/> objects
        /// </summary>
        [JsonProperty("transactions")]
        public IEnumerable<Transaction> Transactions { get; set; }

        /// <summary>
        /// The unique identifier of the user who performed the refund.
        /// </summary>
        [JsonProperty("user_id")]
        public long? UserId { get; set; }

        /// <summary>
        /// A list of duties that have been returned as part of the refund.
        /// </summary>
        [JsonProperty("duties")]
        public IEnumerable<RefundDuty> Duties { get; set; }

        /// <summary>
        /// A list of refunded duties
        /// </summary>
        [JsonProperty("refund_duties")]
        public IEnumerable<RefundDutyType> RefundDuties { get; set; }
        
    }

    public class Shipping
    {
        /// <summary>
        /// Whether to refund all remaining shipping.
        /// </summary>
        [JsonProperty("full_refund")]
        public bool? FullRefund { get; set; }

        /// <summary>
        /// Set a specific amount to refund for shipping. Takes precedence over full_refund.
        /// </summary>
        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        /// <summary>
        /// The maximum amount that can be refunded
        /// </summary>
        [JsonProperty("maximum_refundable")]
        public decimal? MaximumRefundable { get; set; }
    }
}
