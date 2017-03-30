﻿using Machine.Specifications;
using ShopifySharp.Tests.Test_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifySharp.Tests
{
    [Subject(typeof(CustomerService))]
    public class When_updating_a_customer_with_options
    {
        Establish context = () =>
        {
            _Service = new CustomerService(Utils.MyShopifyUrl, Utils.AccessToken);

            //Create a customer to update
            _Customer = _Service.CreateAsync(CustomerCreation.CreateValidCustomer()).Await().AsTask.Result;

            //Change the customer's email address
            _Customer.Email = "test-update@example.com";
        };

        Because of = () =>
        {
            _Customer = _Service.UpdateAsync(_Customer, new CustomerUpdateOptions()
            {
                Password = "loktarogar",
                PasswordConfirmation = "loktarogar"
            }).Await().AsTask.Result;
        };

        It should_update_the_customer_with_options = () =>
        {
            _Customer.Email.Equals("test-update@example.com").ShouldBeTrue();
        };

        Cleanup after = () =>
        {
            _Service.DeleteAsync(_Customer.Id.Value).Await();
        };

        static CustomerService _Service;
        static Customer _Customer;
    }
}
