﻿using System;
using System.Threading.Tasks;
using Xunit;

namespace ShopifySharp.Tests
{
    [Trait("Category", "ApplicationCredit")]
    public class ApplicationCredit_Tests
    {
        private ApplicationCreditService _Service = new ApplicationCreditService(Utils.MyShopifyUrl, Utils.AccessToken);

        public ApplicationCredit_Tests()
        {

        }

        private ApplicationCredit CreateCredit()
        {
            return new ApplicationCredit()
            {
                Description = "Refund for Foo",
                Amount = 10.00m,
                Test = true,
            };
        }

        [Fact(Skip = "Application Credits cannot be tested because they're unusable in a private application.")]
        public async Task Creates_An_Application_Credit()
        {
            var credit = await _Service.CreateAsync(CreateCredit());

            Assert.NotNull(credit);
            Assert.True(credit.Id.HasValue);
            Assert.NotEmpty(credit.Description);
            Assert.True(credit.Test.Value);
            Assert.True(credit.Amount > 0);
        }
    }
}
