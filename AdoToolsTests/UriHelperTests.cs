using System;
using NUnit.Framework;

namespace DevOpsTools.UnitTests
{
    [TestFixture]
    public static class UriHelperTests
    {
        [TestCase("https://www.google.com", "", "https://www.google.com")]
        [TestCase("https://www.google.com", "queryString", "https://www.google.com?queryString")]
        public static void CombineUriParts_ReturnsExpectedResults(
            string basePath,
            string queryString,
            string expectedResult)
        {
            var result = UriHelper.CombineUriParts(basePath, queryString);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public static void CombineUriParts_Throws(
            string basePath
        )
        {
            Assert.That(
                () => UriHelper.CombineUriParts(basePath, ""),
                Throws.TypeOf<ArgumentException>()
            );
        }

        [TestCase("")]
        [TestCase("a", "a")]
        [TestCase("a/b", "a", "b")]
        [TestCase("a/b/c", "a", "b", "c")]
        public static void CombineUriPath_ReturnsExpectedResult(string expectedResult, params string[] inputs)
        {
            var result = UriHelper.CombineUriPath(inputs);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("")]
        [TestCase("a=1", "a=1")]
        [TestCase("a=1&b=2", "a=1", "b=2")]
        [TestCase("a=1&b=2&c=3", "a=1", "b=2", "c=3")]
        public static void CombineUriQuery_ReturnsExpectedResult(string expectedResult, params string[] inputs)
        {
            var result = UriHelper.CombineUriQuery(inputs);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}