﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifySharp
{
    public class ShopifyMetaField
    {
        /// <summary>
        /// Identifier for the metafield (maximum of 30 characters).
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Information to be stored as metadata. Must be either a string or an int.
        /// </summary>
        [JsonProperty("value")]
        public object Value { get; set; }

        /// <summary>
        /// States whether the information in the value is stored as a 'string' or 'integer.'
        /// </summary>
        [JsonProperty("value_type")]
        public string ValueType { get; set; }

        /// <summary>
        /// Container for a set of metadata. Namespaces help distinguish between metadata you created and metadata created by another individual with a similar namespace (maximum of 20 characters).
        /// </summary>
        [JsonProperty("namespace")]
        public object Namespace { get; set; }

        /// <summary>
        /// Additional information about the metafield.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
