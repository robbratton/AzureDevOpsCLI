using System;
using NUnit.Framework;

namespace VSTSTool.UnitTests
{
    [TestFixture]
    public static class AppSettingsTests
    {
        [Test]
        public static void ConstructorSucceeds()
        {
            var result = new AppSettings();

            Assert.That(result, Is.Not.Null);

            Assert.That(result.AzureDevOpsOrganization, Is.Not.Null);
            Assert.That(result.AzureDevOpsProject, Is.Not.Null);
        }
    }
}