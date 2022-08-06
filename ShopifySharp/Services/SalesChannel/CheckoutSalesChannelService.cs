﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShopifySharp.Entities.SalesChannel;
using ShopifySharp.Infrastructure;

namespace ShopifySharp
{
    /// <summary>
    ///  A service use the Checkout API to let customers purchase products from Shopify stores that have installed your sales channel.
    /// </summary>
    public class CheckoutSalesChannelService: ShopifyService
    {
        public CheckoutSalesChannelService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken)
        { }

        /// <summary>
        /// Gets an existing, processing or completed checkout.
        /// </summary>
        public virtual async Task<CheckoutSalesChannel> GetAsync(string token, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"checkouts/{token}.json");

            var response = await ExecuteRequestAsync<CheckoutSalesChannel>(req, HttpMethod.Get, cancellationToken, rootElement: "checkout");
            return response.Result;
        }

        /// <summary>
        /// Creates a new Checkout.
        /// </summary>
        public virtual async Task<CheckoutSalesChannel> CreateAsync(CheckoutSalesChannel checkout,
            CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest("checkouts.json");

            var response = await ExecuteRequestAsync<CheckoutSalesChannel>(req, HttpMethod.Post, cancellationToken,
                new JsonContent(new {checkout}), "checkout");
            return response.Result;
        }

        /// <summary>
        /// Completes a checkout without requiring payment.
        /// </summary>
        public virtual async Task<CheckoutSalesChannel> CompleteAsync(string token, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"checkouts/{token}/complete.json");

            var response = await ExecuteRequestAsync<CheckoutSalesChannel>(req, HttpMethod.Post, cancellationToken, rootElement: "checkout");
            return response.Result;
        }

        /// <summary>
        /// Updates an existing checkout based on the token id.
        /// </summary>
        public virtual async Task<CheckoutSalesChannel> UpdateAsync(string token, CheckoutSalesChannel checkout,
            CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"checkouts/{token}.json");

            var response = await ExecuteRequestAsync<CheckoutSalesChannel>(req, HttpMethod.Put, cancellationToken,
                new JsonContent(new { checkout }), "checkout");
            return response.Result;
        }

        /// <summary>
        /// Retrieves a list of available shipping rates for the specified checkout. Implementers need to poll this endpoint until rates become available. Each shipping rate contains the checkout's 
        /// new subtotal price, total tax, and total price in the event that this shipping rate is selected. This can be used to update the UI without performing further API requests. To apply a 
        /// shipping rate, update the checkout's shipping line with the handle of the selected rate. 
        /// </summary>
        public virtual async Task<IEnumerable<CheckoutShippingRate>> ListShippingRatesAsync(string token, CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"checkouts/{token}/shipping_rates.json");

            var response = await ExecuteRequestAsync<List<CheckoutShippingRate>>(req, HttpMethod.Get, cancellationToken, rootElement: "shipping_rates");
            return response.Result;
        }

        /// <summary>
        /// Creates a payment on a checkout using the session ID returned by the card vault
        /// </summary>
        public virtual async Task<PaymentSalesChannel> CreatePaymentAsync(string token, CreatePayment createPayment,
            CancellationToken cancellationToken = default)
        {
            var req = PrepareRequest($"checkouts/{token}/payments.json");

            var response = await ExecuteRequestAsync<PaymentSalesChannel>(req, HttpMethod.Post, cancellationToken,
                new JsonContent(new {payment = createPayment}), "payment");
            return response.Result;
        }

        /// <summary>
        /// Stores a credit card in the card vault. Credit cards cannot be sent to the Checkout API directly.
        /// They must be sent to the card vault, which in response will return a session ID.
        /// This session ID can then be used when calling the POST #{token}/payments.json endpoint <see cref="CreatePaymentAsync" />.
        /// A session ID is valid only for a single call to the endpoint.
        /// The card vault has a static URL and is located at https://elb.deposit.shopifycs.com/sessions.
        /// It is also provided via the payment_url property on the Checkout resource.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<CardVault> StoreCreditCard(CreditCard creditCard, CancellationToken cancellationToken = default)
        {
            var req = new RequestUri(new Uri("https://elb.deposit.shopifycs.com/sessions"));

            var response = await ExecuteRequestAsync<CardVault>(req, HttpMethod.Post, cancellationToken,
                new JsonContent(new { credit_card = creditCard }));

            return response.Result;
        }
    }
}
