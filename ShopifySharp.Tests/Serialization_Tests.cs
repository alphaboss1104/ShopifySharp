using Newtonsoft.Json;
using ShopifySharp.Infrastructure;
using ShopifySharp.Lists;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = ShopifySharp.Tests.Extensions.EmptyExtensions;

namespace ShopifySharp.Tests
{
    [Trait("Category", "Serialization")]
    public class Serialization_Tests
    {
        [Fact]
        public void DeserializeOrderWithPropertiesAsArray()
        {
            string json = @"
{
  ""id"": 123,
  ""line_items"":
  [
    {
      ""id"": 456,
      ""properties"": []
    }
  ]
}
";
            var order = JsonConvert.DeserializeObject<Order>(json);
            Assert.NotNull(order.LineItems.First().Properties);
        }

        [Fact]
        public void DeserializeOrderWithPropertiesAsObjectInsteadOfArray()
        {
            string json = @"
{
  ""id"": 123,
  ""line_items"":
  [
    {
      ""id"": 456,
      ""properties"": {}
    }
  ]
}
";
            var order = JsonConvert.DeserializeObject<Order>(json);
            Assert.Null(order.LineItems.First().Properties);
        }
    }
}
