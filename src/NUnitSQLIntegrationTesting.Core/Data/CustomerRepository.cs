using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NUnitSQLIntegrationTesting.Core.Infrastructure.Database;
using NUnitSQLIntegrationTesting.Core.Models;

namespace NUnitSQLIntegrationTesting.Core.Data
{
    public class CustomerRepository : SqlServerRepository
    {
        public CustomerRepository(ISqlServerConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<int> CreateCustomerAsync(Customer customer)
        {
            using (var connection = await GetAndOpenConnectionAsync())
            {
                return await connection.QuerySingleAsync<int>(
                    "dbo.CreateCustomer", 
                    new
                    {
                        customer.Name,
                        customer.EmailAddress,
                        customer.CustomerType.CustomerTypeId
                    }, 
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            using (var connection = await GetAndOpenConnectionAsync())
            {
                return (await connection.QueryAsync<Customer, CustomerType, Customer>(
                    "dbo.GetCustomer",
                    (c, ct) =>
                    {
                        c.CustomerType = ct;
                        return c;
                    },
                    new
                    {
                        CustomerId = customerId
                    },
                    commandType: CommandType.StoredProcedure,
                    splitOn: "CustomerTypeId"
                    )).SingleOrDefault();
            }
        }

        public async Task SetCustomerEmailAsync(int customerId, string emailAddress)
        {
            using (var connection = await GetAndOpenConnectionAsync())
            {
                await connection.ExecuteAsync(
                    "dbo.SetCustomerEmailAddress",
                    new
                    {
                        CustomerId = customerId,
                        EmailAddress = emailAddress
                    },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Customer>> GetCustomersByTypeAsync(int customerTypeId)
        {
            using (var connection = await GetAndOpenConnectionAsync())
            {
                return (await connection.QueryAsync<Customer, CustomerType, Customer>(
                    "dbo.GetCustomersByType",
                    (c, ct) =>
                    {
                        c.CustomerType = ct;
                        return c;
                    },
                    new
                    {
                        CustomerTypeId = customerTypeId
                    },
                    commandType: CommandType.StoredProcedure,
                    splitOn: "CustomerTypeId"
                    ));
            }
        }
    }
}
