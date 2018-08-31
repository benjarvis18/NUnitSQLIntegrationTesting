using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitSQLIntegrationTesting.Core.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public CustomerType CustomerType { get; set; }
    }
}
