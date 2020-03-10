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
    /// A service for manipulating Shopify orders.
    /// </summary>
    public class OrderService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="OrderService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public OrderService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a count of all of the shop's orders.
        /// </summary>
        /// <param name="filter">Options for filtering the count.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The count of all orders for the shop.</returns>
        public virtual async Task<int> CountAsync(OrderCountFilter filter = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteGetAsync<int>("orders/count.json", "count", filter, cancellationToken);
        }
        
        /// <summary>
        /// Gets a list of up to 250 of the shop's orders.
        /// </summary>
        /// <param name="filter">Options for filtering the list.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The list of orders matching the filter.</returns>
        public virtual async Task<ListResult<Order>> ListAsync(ListFilter<Order> filter, CancellationToken cancellationToken = default)
        {
            return await ExecuteGetListAsync("orders.json", "orders", filter, cancellationToken);
        }

        /// <summary>
        /// Gets a list of up to 250 of the shop's orders.
        /// </summary>
        /// <param name="filter">Options for filtering the list.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The list of orders matching the filter.</returns>
        public virtual async Task<ListResult<Order>> ListAsync(OrderListFilter filter = null, CancellationToken cancellationToken = default)
        {
            return await ListAsync(filter?.AsListFilter(), cancellationToken);
        }

        /// <summary>
        /// Retrieves the <see cref="Order"/> with the given id.
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The <see cref="Order"/>.</returns>
        public virtual async Task<Order> GetAsync(long orderId, string fields = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteGetAsync<Order>($"orders/{orderId}.json", "order", fields, cancellationToken);
        }

        /// <summary>
        /// Closes an order.
        /// </summary>
        /// <param name="id">The order's id.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task<Order> CloseAsync(long id, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"orders/{id}/close.json");
            var response = await ExecuteRequestAsync<Order>(req, HttpMethod.Post, cancellationToken, rootElement: "order");

            return response.Result;
        }

        /// <summary>
        /// Opens a closed order.
        /// </summary>
        /// <param name="id">The order's id.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task<Order> OpenAsync(long id, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"orders/{id}/open.json");
            var response = await ExecuteRequestAsync<Order>(req, HttpMethod.Post, cancellationToken, rootElement: "order");

            return response.Result;
        }

        /// <summary>
        /// Creates a new <see cref="Order"/> on the store.
        /// </summary>
        /// <param name="order">A new <see cref="Order"/>. Id should be set to null.</param>
        /// <param name="options">Options for creating the order.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The new <see cref="Order"/>.</returns>
        public virtual async Task<Order> CreateAsync(Order order, OrderCreateOptions options = null, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest("orders.json");
            var body = order.ToDictionary();

            if (options != null)
            {
                foreach (var option in options.ToDictionary())
                {
                    body.Add(option);
                }
            }

            var content = new JsonContent(new
            {
                order = body
            });
            var response = await ExecuteRequestAsync<Order>(req, HttpMethod.Post, cancellationToken, content, "order");

            return response.Result;
        }

        /// <summary>
        /// Updates the given <see cref="Order"/>.
        /// </summary>
        /// <param name="orderId">Id of the object being updated.</param>
        /// <param name="order">The <see cref="Order"/> to update.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The updated <see cref="Order"/>.</returns>
        public virtual async Task<Order> UpdateAsync(long orderId, Order order, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"orders/{orderId}.json");
            var content = new JsonContent(new
            {
                order = order
            });
            var response = await ExecuteRequestAsync<Order>(req, HttpMethod.Put, cancellationToken, content, "order");

            return response.Result;
        }

        /// <summary>
        /// Deletes an order with the given Id.
        /// </summary>
        /// <param name="orderId">The order object's Id.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public virtual async Task DeleteAsync(long orderId, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"orders/{orderId}.json");

            await ExecuteRequestAsync(req, HttpMethod.Delete, cancellationToken);
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="orderId">The order's id.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The cancelled <see cref="Order"/>.</returns>
        public virtual async Task CancelAsync(long orderId, OrderCancelOptions options = null, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"orders/{orderId}/cancel.json");
            var content = new JsonContent(options ?? new OrderCancelOptions());

            await ExecuteRequestAsync(req, HttpMethod.Post, cancellationToken, content);
        }

        /// <summary>
        /// Get MetaField's for an order.
        /// </summary>
        /// <param name="orderId">The order's id.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The set of <see cref="MetaField"/> for the order.</returns>
        public virtual async Task<IEnumerable<MetaField>> GetMetaFieldsAsync(long orderId, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"orders/{orderId}/metafields.json");
            var response = await ExecuteRequestAsync<List<MetaField>>(req, HttpMethod.Get, cancellationToken, rootElement: "metafields");

            return response.Result;
        }
    }
}
