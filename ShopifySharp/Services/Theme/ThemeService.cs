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
    /// A service for manipulating Shopify themes.
    /// </summary>
    public class ThemeService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="ThemeService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public ThemeService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a list of up to 250 of the shop's themes.
        /// </summary>
        public virtual async Task<IEnumerable<Theme>> ListAsync(ThemeListFilter filter = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteGetAsync<IEnumerable<Theme>>("themes.json", "themes", filter, cancellationToken);
        }

        /// <summary>
        /// Retrieves the <see cref="Theme"/> with the given id.
        /// </summary>
        /// <param name="themeId">The id of the theme to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The <see cref="Theme"/>.</returns>
        public virtual async Task<Theme> GetAsync(long themeId, string fields = null, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"themes/{themeId}.json");

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            var response = await ExecuteRequestAsync<Theme>(req, HttpMethod.Get, cancellationToken, rootElement: "theme");

            return response.Result;
        }

        private async Task<Theme> _CreateAsync(Theme theme, CancellationToken cancellationToken, string sourceUrl = null)
        {
            var req = PrepareRequest("themes.json");
            var body = theme.ToDictionary();

            if (!string.IsNullOrEmpty(sourceUrl))
            {
                body.Add("src", sourceUrl);
            }

            var content = new JsonContent(new
            {
                theme = body
            });
            var response = await ExecuteRequestAsync<Theme>(req, HttpMethod.Post, cancellationToken, content, "theme");

            return response.Result;
        }

        /// <summary>
        /// Creates a new theme on the store. The theme always starts out with a role of
        /// "unpublished." If the theme has a different role, it will be assigned that only after all of its
        /// files have been extracted and stored by Shopify (which might take a couple of minutes).
        /// </summary>
        /// <param name="theme">The new theme.</param>
        /// <param name="sourceUrl">A URL that points to the .zip file containing the theme's source files.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task<Theme> CreateAsync(Theme theme, CancellationToken cancellationToken = default)
        {
            return await _CreateAsync(theme, cancellationToken);
        }

        /// <summary>
        /// Creates a new theme on the store. The theme always starts out with a role of
        /// "unpublished." If the theme has a different role, it will be assigned that only after all of its
        /// files have been extracted and stored by Shopify (which might take a couple of minutes).
        /// </summary>
        /// <param name="theme">The new theme.</param>
        /// <param name="sourceUrl">A URL that points to the .zip file containing the theme's source files.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task<Theme> CreateAsync(Theme theme, string sourceUrl, CancellationToken cancellationToken = default)
        {
            return await _CreateAsync(theme, cancellationToken, sourceUrl);
        }

        /// <summary>
        /// Updates the given <see cref="Theme"/>.
        /// </summary>
        /// <param name="themeId">Id of the object being updated.</param>
        /// <param name="theme">The <see cref="Theme"/> to update.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The updated <see cref="Theme"/>.</returns>
        public virtual async Task<Theme> UpdateAsync(long themeId, Theme theme, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"themes/{themeId}.json");
            var content = new JsonContent(new
            {
                theme = theme
            });
            var response = await ExecuteRequestAsync<Theme>(req, HttpMethod.Put, cancellationToken, content, "theme");

            return response.Result;
        }

        /// <summary>
        /// Deletes a Theme with the given Id.
        /// </summary>
        /// <param name="themeId">The Theme object's Id.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task DeleteAsync(long themeId, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"themes/{themeId}.json");

            await ExecuteRequestAsync(req, HttpMethod.Delete, cancellationToken);
        }
    }
}
