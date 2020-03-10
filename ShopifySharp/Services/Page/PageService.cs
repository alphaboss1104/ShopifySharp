﻿using System;
using Newtonsoft.Json.Linq;
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
    /// A service for manipulating Shopify pages.
    /// </summary>
    public class PageService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="PageService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public PageService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }
        
        /// <summary>
        /// Gets a count of all of the shop's pages.
        /// </summary>
        public virtual async Task<int> CountAsync(PageCountFilter filter = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteGetAsync<int>("pages/count.json", "count", filter, cancellationToken);
        }

        /// <summary>
        /// Gets a list of up to 250 of the shop's pages.
        /// </summary>
        public virtual async Task<ListResult<Page>> ListAsync(ListFilter<Page> filter, CancellationToken cancellationToken = default)
        {
            return await ExecuteGetListAsync("pages.json", "pages", filter, cancellationToken);
        }

        /// <summary>
        /// Gets a list of up to 250 of the shop's pages.
        /// </summary>
        public virtual async Task<ListResult<Page>> ListAsync(PageListFilter filter = null, CancellationToken cancellationToken = default)
        {
            return await ListAsync(filter?.AsListFilter(), cancellationToken);
        }

        /// <summary>
        /// Retrieves the <see cref="Page"/> with the given id.
        /// </summary>
        /// <param name="pageId">The id of the page to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The <see cref="Page"/>.</returns>
        public virtual async Task<Page> GetAsync(long pageId, string fields = null, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"pages/{pageId}.json");

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            var response = await ExecuteRequestAsync<Page>(req, HttpMethod.Get, cancellationToken, rootElement: "page");

            return response.Result;
        }

        /// <summary>
        /// Creates a new <see cref="Page"/> on the store.
        /// </summary>
        /// <param name="page">A new <see cref="Page"/>. Id should be set to null.</param>
        /// <param name="options">Options for creating the page.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The new <see cref="Page"/>.</returns>
        public virtual async Task<Page> CreateAsync(Page page, PageCreateOptions options = null, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest("pages.json");
            var body = page.ToDictionary();

            if (options != null)
            {
                foreach (var option in options.ToDictionary())
                {
                    body.Add(option);
                }
            }

            var content = new JsonContent(new
            {
                page = body
            });
            var response = await ExecuteRequestAsync<Page>(req, HttpMethod.Post, cancellationToken, content, "page");

            return response.Result;
        }

        /// <summary>
        /// Updates the given <see cref="Page"/>.
        /// </summary>
        /// <param name="pageId">Id of the object being updated.</param>
        /// <param name="page">The <see cref="Page"/> to update.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The updated <see cref="Page"/>.</returns>
        public virtual async Task<Page> UpdateAsync(long pageId, Page page, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"pages/{pageId}.json");
            var content = new JsonContent(new
            {
                page = page
            });
            var response = await ExecuteRequestAsync<Page>(req, HttpMethod.Put, cancellationToken, content, "page");

            return response.Result;
        }

        /// <summary>
        /// Deletes a page with the given Id.
        /// </summary>
        /// <param name="pageId">The page object's Id.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task DeleteAsync(long pageId, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"pages/{pageId}.json");

            await ExecuteRequestAsync(req, HttpMethod.Delete, cancellationToken);
        }
    }
}
