using System;
using DevOpsTools.Tools;
using NUnit.Framework;

namespace DevOpsTools.UnitTests.Tools
{
    [TestFixture]
    public class ToolTests
    {
        private const string FakeUriBase = "https://dev.azure.com/organization/project/_apis/A/B/";

        [TestCase(
            "",
            "",
            FakeUriBase + "?" + BuildDefinitionTool.LocalAPIVersionSuffix)]
        [TestCase(
            "suffix",
            "",
            FakeUriBase + "suffix?" + BuildDefinitionTool.LocalAPIVersionSuffix)]
        [TestCase(
            "",
            "propertyFilters=xyz",
            FakeUriBase + "?propertyFilters=xyz&" + BuildDefinitionTool.LocalAPIVersionSuffix)]
        [TestCase(
            "suffix",
            "propertyFilters=xyz",
            FakeUriBase + "suffix?propertyFilters=xyz&" + BuildDefinitionTool.LocalAPIVersionSuffix)]
        public void MakeUri_returnsExpectedResult(
            string pathSuffix,
            string queryString,
            string expectedResult
        )
        {
            const string basePath =FakeUriBase;
            const string apiVersionSuffix = BuildDefinitionTool.LocalAPIVersionSuffix;

            var result = Helpers.MakeUri(
                basePath,
                apiVersionSuffix, 
                pathSuffix, queryString
                );

            Assert.That(result.ToString(), Is.EqualTo(expectedResult));
        }
    }
}