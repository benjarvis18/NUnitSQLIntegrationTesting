using NUnitSQLIntegrationTesting.Core.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace NUnitSQLIntegrationTesting.Core.Data
{
    public class SqlServerRepository
    {
        private readonly ISqlServerConnectionFactory _connectionFactory;

        public SqlServerRepository(ISqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected async Task<SqlConnection> GetAndOpenConnectionAsync()
        {
            return await _connectionFactory.CreateAndOpenAsync();
        }
    }
}
