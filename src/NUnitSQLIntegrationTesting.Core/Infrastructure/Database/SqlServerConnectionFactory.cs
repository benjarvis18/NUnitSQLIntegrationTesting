using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace NUnitSQLIntegrationTesting.Core.Infrastructure.Database
{
    public class SqlServerConnectionFactory : ISqlServerConnectionFactory
    {
        private readonly string _connectionString = "INJECT FROM CONFIG";

        public async Task<SqlConnection> CreateAndOpenAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            return connection;
        }
    }
}
