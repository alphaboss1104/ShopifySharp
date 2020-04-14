﻿using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShopifySharp.Filters;
using ShopifySharp.Infrastructure;

namespace ShopifySharp
{
    /// <summary>
    /// A service for manipulating Shopify's UsageCharge API.
    /// </summary>
    public class UsageChargeService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="UsageChargeService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public UsageChargeService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Creates a <see cref="UsageCharge"/>.
        /// </summary>
        /// <param name="recurringChargeId">The id of the <see cref="UsageCharge"/> that this usage charge belongs to.</param>
        /// <param name="description">The name or description of the usage charge.</param>
        /// <param name="price">The price of the usage charge.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The new <see cref="UsageCharge"/>.</returns>
        public virtual async Task<UsageCharge> CreateAsync(long recurringChargeId, string description, decimal price, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"recurring_application_charges/{recurringChargeId}/usage_charges.json");
            var content = new JsonContent(new
            {
                usage_charge = new
                {
                    description = description,
                    price = price
                }
            });
            var response = await ExecuteRequestAsync<UsageCharge>(req, HttpMethod.Post, cancellationToken, content, "usage_charge");

            return response.Result;
        }

        /// <summary>
        /// Retrieves the <see cref="UsageCharge"/> with the given id.
        /// </summary>
        /// <param name="recurringChargeId">The id of the recurring charge that this usage charge belongs to.</param>
        /// <param name="id">The id of the charge to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The <see cref="UsageCharge"/>.</returns>
        public virtual async Task<UsageCharge> GetAsync(long recurringChargeId, long id, string fields = null, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"recurring_application_charges/{recurringChargeId}/usage_charges/{id}.json");

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            var response = await ExecuteRequestAsync<UsageCharge>(req, HttpMethod.Get, cancellationToken, rootElement: "usage_charge");

            return response.Result;
        }

        /// <summary>
        /// Retrieves a list of all past and present <see cref="UsageCharge"/> objects.
        /// </summary>
        /// <param name="recurringChargeId">The id of the recurring charge that these usage charges belong to.</param>
        /// <param name="filter">Options for filtering the list.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task<IEnumerable<UsageCharge>> ListAsync(long recurringChargeId, UsageChargeListFilter filter = null, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"recurring_application_charges/{recurringChargeId}/usage_charges.json");

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToQueryParameters());
            }
            
            var response = await ExecuteRequestAsync<List<UsageCharge>>(req, HttpMethod.Get, cancellationToken, rootElement: "usage_charges");

            return response.Result;
        }
    }
}
