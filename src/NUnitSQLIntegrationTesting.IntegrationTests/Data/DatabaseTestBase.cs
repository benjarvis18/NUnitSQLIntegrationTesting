using AutoFixture;
using Moq;
using NUnit.Framework;
using NUnitSQLIntegrationTesting.Core.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NUnitSQLIntegrationTesting.IntegrationTests.Data
{
    public class DatabaseTestBase
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        [SetUp]
        public void SetUp()
        {
            _semaphore.Wait();
        }

        [TearDown]
        public void TearDown()
        {
            DatabaseTestConfig.TestDatabase.RunCleanUpScript();
            _semaphore.Release();
        }

        protected IFixture GetFixture()
        {
            var fixture = FixtureFactory.GetFixture();

            var sqlConnectionFactory = fixture.Freeze<Mock<ISqlServerConnectionFactory>>();
            sqlConnectionFactory
                .Setup(f => f.CreateAndOpenAsync())
                .ReturnsAsync(DatabaseTestConfig.TestDatabase.GetSqlConnection());

            return fixture;
        }
    }
}
