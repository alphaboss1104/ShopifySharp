﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShopifySharp
{
    public class ProductListingVariant: ShopifyObject
    {
        
        /// <summary>
        /// Available current product variant
        /// </summary>
        [JsonProperty("available")]
        public bool? Available { get; set; }

        /// <summary>
        /// Custom properties that a shop owner can use to define product variants.
        /// </summary>
        [JsonProperty("option_values")]
        public IEnumerable<OptionValue> OptionValues { get; set; }

        /// <summary>
        /// The title of the product variant.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// A unique identifier for the product in the shop.
        /// </summary>
        [JsonProperty("sku")]
        public string SKU { get; set; }

        /// <summary>
        /// The order of the product variant in the list of product variants. 1 is the first position.
        /// </summary>
        [JsonProperty("position")]
        public int? Position { get; set; }

        /// <summary>
        /// The weight of the product variant in grams.
        /// </summary>
        [JsonProperty("grams")]
        public long? Grams { get; set; }

        /// <summary>
        /// Specifies whether or not customers are allowed to place an order for a product variant when it's out of stock. Known values are 'deny' and 'continue'.
        /// </summary>
        [JsonProperty("inventory_policy")]
        public string InventoryPolicy { get; set; }

        /// <summary>
        /// Service that is doing the fulfillment. Can be 'manual' or any custom string.
        /// </summary>
        [JsonProperty("fulfillment_service")]
        public string FulfillmentService { get; set; }

        /// <summary>
        /// Specifies whether or not Shopify tracks the number of items in stock for this product variant. Known values are 'blank' and 'shopify'.
        /// </summary>
        [JsonProperty("inventory_management")]
        public string InventoryManagement { get; set; }

        /// <summary>
        /// The price of the product variant.
        /// </summary>
        [JsonProperty("price")]
        public decimal? Price { get; set; }

        /// <summary>
        /// The competitors prices for the same item.
        /// </summary>
        [JsonProperty("compare_at_price", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public decimal? CompareAtPrice { get; set; }

        /// <summary>
        /// The date and time when the product variant was created. The API returns this value in ISO 8601 format.
        /// </summary>
        [JsonProperty("created_at", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the product variant was last modified. The API returns this value in ISO 8601 format.
        /// </summary>
        [JsonProperty("updated_at", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// Specifies whether or not a tax is charged when the product variant is sold.
        /// </summary>
        [JsonProperty("taxable")]
        public bool? Taxable { get; set; }

        /// <summary>
        /// Specifies whether or not a customer needs to provide a shipping address when placing an order for this product variant.
        /// </summary>
        [JsonProperty("requires_shipping")]
        public bool? RequiresShipping { get; set; }

        /// <summary>
        /// The barcode, UPC or ISBN number for the product.
        /// </summary>
        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// The number of items in stock for this product variant.
        /// NOTE: After 2018-07-01, this field will be read-only in the Shopify API. Use the `InventoryLevelService` instead.
        /// </summary>
        [JsonProperty("inventory_quantity")]
        public long? InventoryQuantity { get; set; }

        /// <summary>
        /// The unique numeric identifier for one of the product's images.
        /// </summary>
        [JsonProperty("image_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? ImageId { get; set; }

        /// <summary>
        /// The weight of the product variant in the unit system specified with weight_unit.
        /// </summary>
        [JsonProperty("weight")]
        public decimal? Weight { get; set; }

        /// <summary>
        /// The unit system that the product variant's weight is measure in. The weight_unit can be either "g", "kg, "oz", or "lb".
        /// </summary>
        [JsonProperty("weight_unit")]
        public string WeightUnit { get; set; }
    }
}