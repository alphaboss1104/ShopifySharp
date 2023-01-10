﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShopifySharp.Tests.SalesChannel
{
    [Trait("Category", "CollectionListing")]
    public class CollectionListing_Tests : IClassFixture<CollectionListing_Tests_Fixture>
    {
        private CollectionListing_Tests_Fixture Fixture { get; }

        public CollectionListing_Tests(CollectionListing_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact(Skip = "Sales channel tests cannot be run with ShopifySharp's private/custom app.")]
        public async Task Lists_Collections_NoFilter()
        {
            var list = await Fixture.Service.ListAsync();

            Assert.True(list.Items.Any());
            if (list.LinkHeader != null)
            {
                Assert.NotNull(list.LinkHeader.NextLink);
                Assert.NotNull(list.LinkHeader.NextLink.PageInfo);
                Assert.NotNull(list.LinkHeader.NextLink.Url);
            }
        }

        [Fact(Skip = "Sales channel tests cannot be run with ShopifySharp's private/custom app.")]
        public async Task Lists_Collection_ProductIds_NoFilter()
        {
            var list = await Fixture.Service.ListAsync();
            var actual = list.Items.First();

            var productIds = await Fixture.Service.ListIdsAsync(actual.CollectionId.Value);
            
            Assert.True(productIds.Items.Any());
            if (list.LinkHeader != null)
            {
                Assert.NotNull(list.LinkHeader.NextLink);
                Assert.NotNull(list.LinkHeader.NextLink.PageInfo);
                Assert.NotNull(list.LinkHeader.NextLink.Url);
            }
        }

        [Fact(Skip = "Sales channel tests cannot be run with ShopifySharp's private/custom app.")]
        public async Task Retrieve_Specific_Collection()
        {
            var list = await Fixture.Service.ListAsync();
            var actual = list.Items.First();

            var extected = await Fixture.Service.GetAsync(actual.CollectionId.Value);

            Assert.NotNull(extected);
            Assert.Equal(extected.CollectionId, actual.CollectionId);
            Assert.Equal(extected.Handle, actual.Handle);
        }
    }

    public class CollectionListing_Tests_Fixture : IAsyncLifetime
    {
        public CollectionListingService Service { get; } = new CollectionListingService(Utils.MyShopifyUrl, Utils.AccessToken);

        public Task InitializeAsync()
        {
            Service.SetExecutionPolicy(new LeakyBucketExecutionPolicy());
            
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
