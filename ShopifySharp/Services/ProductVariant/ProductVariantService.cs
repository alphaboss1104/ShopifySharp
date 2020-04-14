﻿using System;
using System.Net.Http;
using ShopifySharp.Filters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShopifySharp.Infrastructure;
using ShopifySharp.Lists;

namespace ShopifySharp
{
    /// <summary>
    /// A service for manipulating a Shopify product's variants.
    /// </summary>
    public class ProductVariantService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="ProductVariantService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public ProductVariantService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a count of all variants belonging to the given product.
        /// </summary>
        /// <param name="productId">The product that the variants belong to.</param>
        /// <param name="filter">Options for filtering the result.</param>
        /// <remarks>
        /// According to Shopify's documentation, this endpoint does not currently support any additional filter parameters for counting.
        /// </remarks>
        public virtual async Task<int> CountAsync(long productId, CancellationToken cancellationToken = default)
        {
            return await ExecuteGetAsync<int>($"products/{productId}/variants/count.json", "count", cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Gets a list of variants belonging to the given product.
        /// </summary>
        /// <param name="productId">The product that the variants belong to.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task<ListResult<ProductVariant>> ListAsync(long productId, ListFilter<ProductVariant> filter, CancellationToken cancellationToken = default)
        {
            return await ExecuteGetListAsync($"products/{productId}/variants.json", "variants", filter, cancellationToken);
        }

        /// <summary>
        /// Gets a list of variants belonging to the given product.
        /// </summary>
        /// <param name="productId">The product that the variants belong to.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task<ListResult<ProductVariant>> ListAsync(long productId, ProductVariantListFilter filter = null, CancellationToken cancellationToken = default)
        {
            return await ListAsync(productId, filter?.AsListFilter(), cancellationToken);
        }

        /// <summary>
        /// Retrieves the <see cref="ProductVariant"/> with the given id.
        /// </summary>
        /// <param name="variantId">The id of the product variant to retrieve.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task<ProductVariant> GetAsync(long variantId, CancellationToken cancellationToken = default)
        {
            return await ExecuteGetAsync<ProductVariant>($"variants/{variantId}.json", "variant", cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates a new <see cref="ProductVariant"/>.
        /// </summary>
        /// <param name="productId">The product that the new variant will belong to.</param>
        /// <param name="variant">A new <see cref="ProductVariant"/>. Id should be set to null.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task<ProductVariant> CreateAsync(long productId, ProductVariant variant, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"products/{productId}/variants.json");
            var content = new JsonContent(new
            {
                variant = variant
            });
            var response = await ExecuteRequestAsync<ProductVariant>(req, HttpMethod.Post, cancellationToken, content, "variant");

            return response.Result;
        }

        /// <summary>
        /// Updates the given <see cref="ProductVariant"/>.
        /// </summary>
        /// <param name="productVariantId">Id of the object being updated.</param>
        /// <param name="variant">The variant to update.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task<ProductVariant> UpdateAsync(long productVariantId, ProductVariant variant, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"variants/{productVariantId}.json");
            var content = new JsonContent(new
            {
                variant = variant
            });
            var response = await ExecuteRequestAsync<ProductVariant>(req, HttpMethod.Put, cancellationToken, content, "variant");

            return response.Result;
        }

        /// <summary>
        /// Deletes a product variant with the given Id.
        /// </summary>
        /// <param name="productId">The product that the variant belongs to.</param>
        /// <param name="variantId">The product variant's id.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task DeleteAsync(long productId, long variantId, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"products/{productId}/variants/{variantId}.json");

            await ExecuteRequestAsync(req, HttpMethod.Delete, cancellationToken);
        }
    }
}
