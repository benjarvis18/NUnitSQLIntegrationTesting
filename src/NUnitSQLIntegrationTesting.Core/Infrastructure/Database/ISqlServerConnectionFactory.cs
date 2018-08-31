using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NUnitSQLIntegrationTesting.Core.Infrastructure.Database
{
    public interface ISqlServerConnectionFactory
    {
        Task<SqlConnection> CreateAndOpenAsync();
    }
}