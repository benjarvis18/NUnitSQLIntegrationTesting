using Moq;
using NUnit.Framework;
using NUnitSQLIntegrationTesting.Core.Data;
using NUnitSQLIntegrationTesting.Core.Infrastructure.Database;
using NUnitSQLIntegrationTesting.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using NUnitSQLIntegrationTesting.IntegrationTests.Helpers;

namespace NUnitSQLIntegrationTesting.IntegrationTests.Data
{
    public class CustomerRepositoryTests : DatabaseTestBase
    {
        [Test]
        public async Task CreateCustomerAsync_WhenGivenAValidCustomer_DatabaseRowIsInserted()
        {
            var fixture = GetFixture();
            var repository = fixture.Create<CustomerRepository>();

            var testCustomer = new Customer()
            {
                CustomerType = new CustomerType() { CustomerTypeId = 1 },
                EmailAddress = "test@testing.com",
                Name = "Mr Test"
            };

            var customerId = await repository.CreateCustomerAsync(testCustomer);

            Helpers.Assert.SqlResultMatches(
                new
                {
                    testCustomer.Name,
                    testCustomer.EmailAddress,
                    testCustomer.CustomerType.CustomerTypeId
                }, 
                $"SELECT Name, EmailAddress, CustomerTypeId FROM dbo.Customer WHERE CustomerId = {customerId}"
            );
        }

        [Test]
        public async Task GetCustomerAsync_WhenGivenAValidCustomerId_ReturnsCustomer()
        {
            var fixture = GetFixture();
            var repository = fixture.Create<CustomerRepository>();

            DatabaseTestConfig.TestDatabase.RunSetUpScript(
                "SET IDENTITY_INSERT dbo.Customer ON;" +
                "INSERT INTO dbo.Customer (CustomerId, Name, EmailAddress, CustomerTypeId)" +
                "VALUES " +
                "(1, 'Test Customer', 'test.customer@testing.com', 1), " +
                "(2, 'Test Customer 2', 'test.customer2@testing.com', 2)" +
                "SET IDENTITY_INSERT dbo.Customer OFF;"
            );

            var customer = await repository.GetCustomerAsync(1);

            Helpers.Assert.SqlResultMatches(
                "SELECT C.CustomerId, " +
                "       C.Name, " +
                "       C.EmailAddress, " +
                "       CT.CustomerTypeId, " +
                "       CT.CustomerTypeName " +
                "FROM   dbo.Customer C " +
                "       INNER JOIN dbo.CustomerType CT ON CT.CustomerTypeId = C.CustomerTypeId " +
                "WHERE  C.CustomerId = 1",
                new
                {
                    customer.CustomerId,
                    customer.Name,
                    customer.EmailAddress,
                    customer.CustomerType.CustomerTypeId,
                    customer.CustomerType.CustomerTypeName
                }
            );
        }

        [Test]
        public async Task SetCustomerEmailAddress_WhenGivenAValidCustomerIdAndEmail_DatabaseRowIsUpdated()
        {
            var fixture = GetFixture();
            var repository = fixture.Create<CustomerRepository>();

            DatabaseTestConfig.TestDatabase.RunSetUpScript(
                "SET IDENTITY_INSERT dbo.Customer ON;" +
                "INSERT INTO dbo.Customer (CustomerId, Name, EmailAddress, CustomerTypeId)" +
                "VALUES " +
                "(1, 'Test Customer', 'test.customer@testing.com', 1), " +
                "(2, 'Test Customer 2', 'test.customer2@testing.com', 2)" +
                "SET IDENTITY_INSERT dbo.Customer OFF;"
            );

            await repository.SetCustomerEmailAsync(1, "test.new.email@testing.com");

            Helpers.Assert.SqlResultMatches(
                new
                {
                    CustomerId = 1
                },
                "SELECT CustomerId FROM dbo.Customer WHERE EmailAddress = 'test.new.email@testing.com'"
            );
        }

        [Test]
        public async Task GetCustomersByTypeAsync_WhenGivenAValidCustomerTypeId_ReturnsCollectionOfCustomers()
        {
            var fixture = GetFixture();
            var repository = fixture.Create<CustomerRepository>();

            DatabaseTestConfig.TestDatabase.RunSetUpScript(
                "SET IDENTITY_INSERT dbo.Customer ON;" +
                "INSERT INTO dbo.Customer (CustomerId, Name, EmailAddress, CustomerTypeId)" +
                "VALUES " +
                "(1, 'Test Customer', 'test.customer@testing.com', 1), " +
                "(2, 'Test Customer 2', 'test.customer2@testing.com', 2)," +
                "(3, 'Test Customer 3', 'test.customer3@testing.com', 2)" +
                "SET IDENTITY_INSERT dbo.Customer OFF;"
            );

            var customers = await repository.GetCustomersByTypeAsync(2);

            Helpers.Assert.SqlMultiRowResultMatches(
                "SELECT C.CustomerId, " +
                "       C.Name, " +
                "       C.EmailAddress, " +
                "       CT.CustomerTypeId, " +
                "       CT.CustomerTypeName " +
                "FROM   dbo.Customer C " +
                "       INNER JOIN dbo.CustomerType CT ON CT.CustomerTypeId = C.CustomerTypeId " +
                "WHERE  C.CustomerTypeId = 2",
                customers.Select(
                    c => new
                    {
                        c.CustomerId,
                        c.Name,
                        c.EmailAddress,
                        c.CustomerType.CustomerTypeId,
                        c.CustomerType.CustomerTypeName
                    }
                ).ToList()
            );
        }
    }
}
