using Dapper;
using Dynamitey;
using NUnitSQLIntegrationTesting.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitSQLIntegrationTesting.IntegrationTests.Helpers
{
    public static class Assert
    {
        public static void SqlResultMatches(dynamic expectedResult, string actualResultQuery)
        {
            using (var connection = DatabaseTestConfig.TestDatabase.GetSqlConnection())
            {
                connection.Open();

                var queryResult = connection.Query(actualResultQuery).FirstOrDefault();

                var properties = Dynamic.GetMemberNames(expectedResult);

                foreach (var property in properties)
                {
                    var expected = Dynamic.InvokeGet(expectedResult, property);
                    var actual = Dynamic.InvokeGet(queryResult, property);

                    NUnit.Framework.Assert.AreEqual(expected, actual, property);
                }
            }
        }

        public static void SqlResultMatches(string expectedResultQuery, dynamic actualResult)
        {
            using (var connection = DatabaseTestConfig.TestDatabase.GetSqlConnection())
            {
                connection.Open();

                var queryResult = connection.Query(expectedResultQuery).FirstOrDefault();

                var properties = Dynamic.GetMemberNames(actualResult);

                foreach (var property in properties)
                {
                    var expected = Dynamic.InvokeGet(queryResult, property);
                    var actual = Dynamic.InvokeGet(actualResult, property);

                    NUnit.Framework.Assert.AreEqual(expected, actual, property);
                }
            }
        }

        public static void SqlMultiRowResultMatches<T>(string expectedQuery, List<T> actualResult)
        {
            using (var connection = DatabaseTestConfig.TestDatabase.GetSqlConnection())
            {
                connection.Open();

                var expectedResult = connection.Query(expectedQuery).ToList();

                NUnit.Framework.Assert.AreEqual(expectedResult.Count, actualResult.Count, "Total Row Count");

                for (int i = 0; i < expectedResult.Count; i++)
                {
                    var expectedObject = expectedResult[i];
                    var actualObject = actualResult[i];

                    var properties = Dynamic.GetMemberNames(actualObject);

                    foreach (var property in properties)
                    {
                        var expected = Dynamic.InvokeGet(expectedObject, property);
                        var actual = Dynamic.InvokeGet(actualObject, property);

                        NUnit.Framework.Assert.AreEqual(expected, actual, $"Row {i + 1} - {property}");
                    }
                }

            }
        }

        public static void SqlMultiRowResultMatches<T>(List<T> expectedResult, string actualQuery)
        {
            using (var connection = DatabaseTestConfig.TestDatabase.GetSqlConnection())
            {
                connection.Open();

                var actualResult = connection.Query(actualQuery).ToList();

                NUnit.Framework.Assert.AreEqual(expectedResult.Count, actualResult.Count, "Total Row Count");

                for (int i = 0; i < expectedResult.Count; i++)
                {
                    var expectedObject = expectedResult[i];
                    var actualObject = actualResult[i];

                    var properties = Dynamic.GetMemberNames(expectedObject);

                    foreach (var property in properties)
                    {
                        var expected = Dynamic.InvokeGet(expectedObject, property);
                        var actual = Dynamic.InvokeGet(actualObject, property);

                        NUnit.Framework.Assert.AreEqual(expected, actual, $"Row {i + 1} - {property}");
                    }
                }

            }
        }
    }
}
