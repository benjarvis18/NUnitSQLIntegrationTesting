using NUnit.Framework;
using NUnitSQLIntegrationTesting.IntegrationTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitSQLIntegrationTesting.IntegrationTests.Data
{
    [SetUpFixture]
    public class DatabaseTestConfig
    {
        public static TestDatabase TestDatabase { get; private set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            TestDatabase = new TestDatabase();
            TestDatabase.Initialise(@"..\..\..\NUnitSQLIntegrationTesting.Database\bin\debug\NUnitSQLIntegrationTesting.Database.dacpac");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (TestDatabase != null)
                TestDatabase.Dispose();
        }
    }
}
