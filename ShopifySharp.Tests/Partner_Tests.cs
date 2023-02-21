using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShopifySharp.Tests
{
    [Trait("Category", "Partners")]
    public class Partner_Tests
    {
        private async Task<JToken> Query(string query, IRequestExecutionPolicy policy = null)
        {
            var svc = new PartnerService(Utils.OrganizationId, Utils.OrganizationToken);

            if (policy != null)
                svc.SetExecutionPolicy(policy);

            return await svc.PostAsync(query);
        }

        private async Task<JToken> QueryTransactions(IRequestExecutionPolicy policy = null)
        {
            string query = @"
{
  transactions
  {
    edges
    {
      node
      {
        id
        createdAt
      }
    }
  }
}";
            return await Query(query, policy);
        }

        [Fact]
        public async Task Transactions()
        {
            var res = await QueryTransactions();
            Assert.NotNull(res);
        }

        [Fact]
        public async Task InvalidQueryMustThrowException()
        {
            await Assert.ThrowsAsync<ShopifyException>(() => Query("{{ query }"));
        }

        [Fact]
        public async Task LeakyBucketPolicyShouldRetry()
        {
            var policy = new LeakyBucketExecutionPolicy();
            bool caught = false;

            try
            {
                //issue 10 parallel requests to trip the 4 req/sec limit
                //they will be retried internally
                await Task.WhenAll(Enumerable.Range(0, 10).Select(async _ => await QueryTransactions(policy)));
            }
            catch (ShopifyRateLimitException)
            {
                caught = true;
            }

            Assert.False(caught);
        }
    }
}
