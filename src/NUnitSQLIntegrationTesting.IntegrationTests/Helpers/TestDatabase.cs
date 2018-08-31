using Dapper;
using Microsoft.SqlServer.Dac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitSQLIntegrationTesting.IntegrationTests.Helpers
{
    public class TestDatabase : IDisposable
    {
        private string _databaseName;
        private string _connectionString;

        private bool _initialised = false;

        public void Initialise(string dacpacRelativePath)
        {
            _databaseName = $"UnitTestDB_{Guid.NewGuid().ToString("N").ToUpper()}";
            var connectionString = $@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;";

            var instance = new DacServices(connectionString);
            var dacpacPath = Path.GetFullPath(
                Path.Combine(
                    TestContext.CurrentContext.TestDirectory,
                    dacpacRelativePath
                ));

            using (var dacpac = DacPackage.Load(dacpacPath))
            {
                instance.Deploy(dacpac, _databaseName, upgradeExisting: true, options: new DacDeployOptions() { AllowIncompatiblePlatform = true });
            }

            _connectionString = string.Concat(connectionString, $"Initial Catalog={_databaseName}");
            _initialised = true;
        }

        private void Drop()
        {
            if (!_initialised)
            {
                return;
            }

            using (var connection = GetSqlConnection())
            {
                connection.Open();
                connection.ChangeDatabase("master");

                using (var command = new SqlCommand($"ALTER DATABASE [{_databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{_databaseName}]", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RunSetUpScript(string sql)
        {
            using (var connection = GetSqlConnection())
            {
                connection.Execute(sql);
            }
        }

        public void RunCleanUpScript()
        {
            using (var connection = GetSqlConnection())
            {
                var sql =
                    "DELETE FROM dbo.Customer;";

                connection.Execute(sql);
            }
        }

        public SqlConnection GetSqlConnection()
        {
            if (_initialised)
            {
                return new SqlConnection(_connectionString);
            }

            return null;
        }

        public void Dispose()
        {
            Drop();
        }
    }
}
