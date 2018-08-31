using AutoFixture;
using AutoFixture.AutoMoq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitSQLIntegrationTesting.IntegrationTests
{
    public static class FixtureFactory
    {
        public static IFixture GetFixture()
        {
            return new Fixture().Customize(new AutoMoqCustomization());
        }
    }
}
