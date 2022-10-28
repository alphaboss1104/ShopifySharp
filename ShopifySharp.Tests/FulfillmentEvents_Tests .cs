using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShopifySharp.Tests
{
    [Trait("Category", "FulfillmentEvent")]
    public class FulfillmentEvents_Tests : IClassFixture<FulfillmentEvents_Tests_Fixture>
    {
        private FulfillmentEvents_Tests_Fixture Fixture { get; }

        public FulfillmentEvents_Tests(FulfillmentEvents_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public async Task Lists_FulfillmentEvents()
        {
            long orderId = Fixture.CreatedFulfillments.First().OrderId.Value;
            long fulfillmentId = Fixture.CreatedFulfillments.First().Id.Value;
            var list = await Fixture.FulfillmentEventService.ListAsync(orderId, fulfillmentId);
            Assert.True(list.Count() > 0);
        }

        [Fact]
        public async Task CreatesAndDeletes_FulfillmentEvents()
        {
            long orderId = Fixture.CreatedFulfillments.First().OrderId.Value;
            long fulfillmentId = Fixture.CreatedFulfillments.First().Id.Value;
            var @event = await Fixture.CreateFulfillmentEvent(orderId, fulfillmentId);

            Assert.NotNull(@event);
            Assert.True(@event.Id.HasValue);
            Assert.Equal("confirmed", @event.Status);

            await Fixture.FulfillmentEventService.DeleteAsync(orderId, fulfillmentId, @event.Id.Value);
        }
    }

    public class FulfillmentEvents_Tests_Fixture : IAsyncLifetime
    {
        public FulfillmentEventService FulfillmentEventService { get; } = new FulfillmentEventService(Utils.MyShopifyUrl, Utils.AccessToken);

        public FulfillmentService FulfillmentService { get; } = new FulfillmentService(Utils.MyShopifyUrl, Utils.AccessToken);
        
        public FulfillmentOrderService FulfillmentOrderService { get; } = new FulfillmentOrderService(Utils.MyShopifyUrl, Utils.AccessToken);

        public OrderService OrderService { get; } = new OrderService(Utils.MyShopifyUrl, Utils.AccessToken);

        public long LocationId => 6226758;

        /// <summary>
        /// Fulfillments must be part of an order and cannot be deleted.
        /// </summary>
        public List<Order> CreatedOrders { get; } = new List<Order>();

        public List<Fulfillment> CreatedFulfillments { get; } = new List<Fulfillment>();

        public List<FulfillmentEvent> CreatedFulfillmentEvents { get; } = new List<FulfillmentEvent>();

        public async Task InitializeAsync()
        {
            var policy = new LeakyBucketExecutionPolicy(false);

            // Fulfillment API has a stricter rate limit when on a non-paid store.
            FulfillmentService.SetExecutionPolicy(policy);
            FulfillmentOrderService.SetExecutionPolicy(policy);
            FulfillmentEventService.SetExecutionPolicy(policy);
            OrderService.SetExecutionPolicy(policy);

            // Create an order and fulfillment for count, list, get, etc. tests.
            var order = await CreateOrder();
            var fulfillment = await CreateFulfillment(order.Id.Value);
            await CreateFulfillmentEvent(order.Id.Value, fulfillment.Id.Value);
        }

        public async Task DisposeAsync()
        {
            foreach (var obj in CreatedOrders)
            {
                try
                {
                    await OrderService.DeleteAsync(obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    Console.WriteLine($"Failed to delete order with id {obj.Id.Value}. {ex.Message}");
                }
            }
        }

        public async Task<Order> CreateOrder()
        {
            var obj = await OrderService.CreateAsync(new Order()
            {
                CreatedAt = DateTime.UtcNow,
                BillingAddress = new Address()
                {
                    Address1 = "123 4th Street",
                    City = "Minneapolis",
                    Province = "Minnesota",
                    ProvinceCode = "MN",
                    Zip = "55401",
                    Phone = "555-555-5555",
                    FirstName = "John",
                    LastName = "Doe",
                    Company = "Tomorrow Corporation",
                    Country = "United States",
                    CountryCode = "US",
                    Default = true,
                },
                LineItems = new List<LineItem>()
                {
                    new LineItem()
                    {
                        Name = "Test Line Item",
                        Title = "Test Line Item Title",
                        Quantity = 2,
                        Price = 5
                    },
                    new LineItem()
                    {
                        Name = "Test Line Item 2",
                        Title = "Test Line Item Title 2",
                        Quantity = 2,
                        Price = 5
                    }
                },
                FinancialStatus = "paid",
                TotalPrice = 5.00m,
                Email = Guid.NewGuid().ToString() + "@example.com",
                Note = "Test note about the customer.",
                Test = true
            }, new OrderCreateOptions()
            {
                SendReceipt = false,
                SendFulfillmentReceipt = false
            });

            CreatedOrders.Add(obj);

            return obj;
        }

        public async Task<IEnumerable<FulfillmentOrder>> ListFulfillmentOrders(long orderId)
        {
            var orders = await FulfillmentOrderService.ListAsync(orderId);

            return orders;
        }

        public async Task<Fulfillment> CreateFulfillment(long orderId)
        {
            var fulfillmentOrders = await ListFulfillmentOrders(orderId);
            var fulfillment = await FulfillmentService.CreateAsync(new FulfillmentShipping
            {
                Message = "Items are shipping now!",
                FulfillmentRequestOrderLineItems = fulfillmentOrders.Select(o => new LineItemsByFulfillmentOrder
                {
                    FulfillmentOrderId = o.Id.Value
                }),
                NotifyCustomer = false,
                TrackingInfo = new TrackingInfo
                {
                    Company = "Jack Black's Pack, Stack and Track",
                    Url = "https://example.com/123456789",
                    Number = "123456789"
                }
            });

            CreatedFulfillments.Add(fulfillment);

            return fulfillment;
        }

        public async Task<FulfillmentEvent> CreateFulfillmentEvent(long orderId, long fulfillmentId)
        {
            var @event = new FulfillmentEvent()
            {
                OrderId = orderId,
                FulfillmentId = fulfillmentId,
                Status = "confirmed"
            };

            @event = await FulfillmentEventService.CreateAsync(orderId, fulfillmentId, @event);

            CreatedFulfillmentEvents.Add(@event);

            return @event;
        }
    }
}
