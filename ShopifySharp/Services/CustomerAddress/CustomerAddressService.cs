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
    /// A service for manipulating Shopify customers addresses.
    /// </summary>
    public class CustomerAddressService : ShopifyService
    {
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public CustomerAddressService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a list of up to 250 of the shop customer's addresses.
        /// </summary>
        /// <param name="customerId">The id of the customer to retrieve.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task<ListResult<Address>> ListAsync(long customerId, ListFilter<Address> filter = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteGetListAsync($"customers/{customerId}/addresses.json", "addresses", filter, cancellationToken);
        }

        /// <summary>
        /// Retrieves the <see cref="Address"/> with the given id.
        /// </summary>
        /// <param name="customerId">The id of the customer to retrieve.</param>
        /// <param name="addressId">The id of the customer address to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The <see cref="Address"/>.</returns>
        public virtual async Task<Address> GetAsync(long customerId, long addressId, string fields = null, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"customers/{customerId}/addresses/{addressId}.json");

            if (string.IsNullOrEmpty(fields) == false)
            {
                req.QueryParams.Add("fields", fields);
            }

            var response = await ExecuteRequestAsync<Address>(req, HttpMethod.Get, cancellationToken, rootElement: "customer_address");
            return response.Result;
        }


        /// <summary>
        /// Creates a new <see cref="Address"/> on the store.
        /// </summary>
        /// <param name="customerId">The id of the customer to create address for.</param>
        /// <param name="address">A new <see cref="Address"/>. Id should be set to null.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The new <see cref="Address"/>.</returns>
        public virtual async Task<Address> CreateAsync(long customerId, Address address, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"customers/{customerId}/addresses.json");
            var addressBody = address.ToDictionary();
            var content = new JsonContent(new
            {
                address = addressBody
            });

            var response = await ExecuteRequestAsync<Address>(req, HttpMethod.Post, cancellationToken, content, "customer_address");
            return response.Result;
        }

        /// <summary>
        /// Updates the given <see cref="Address"/>.
        /// </summary>
        /// <param name="customerId">Id of the customer object being updated.</param>
        /// <param name="addressId">Id of the address object being updated.</param>
        /// <param name="address">The <see cref="Address"/> to update.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The updated <see cref="Customer"/>.</returns>
        public virtual async Task<Address> UpdateAsync(long customerId, long addressId, Address address, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"customers/{customerId}/addresses/{addressId}.json");
            var addressBody = address.ToDictionary();

            var content = new JsonContent(new
            {
                address = addressBody
            });

            var response = await ExecuteRequestAsync<Address>(req, HttpMethod.Put, cancellationToken, content, "customer_address");
            return response.Result;
        }

        /// <summary>
        /// Deletes a address with the given Id from a customer.
        /// </summary>
        /// <param name="customerId">The customer object's Id.</param>
        /// <param name="addressId">The address object's Id.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task DeleteAsync(long customerId, long addressId, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"customers/{customerId}/addresses/{addressId}.json");

            await ExecuteRequestAsync(req, HttpMethod.Delete, cancellationToken);
        }

        /// <summary>
        /// Sets the specified address as the default for a customer
        /// </summary>
        /// <param name="customerId">The customer object's Id.</param>
        /// <param name="addressId">The address object's Id.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<Address> SetDefault(long customerId, long addressId, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"customers/{customerId}/addresses/{addressId}/default.json");

            var response = await ExecuteRequestAsync<Address>(req, HttpMethod.Put, cancellationToken, rootElement: "customer_address");
            return response.Result;
        }
    }
}
