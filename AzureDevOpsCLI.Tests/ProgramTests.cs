using System;
using NUnit.Framework;

namespace VSTSTool.UnitTests
{
    [TestFixture]
    public static class ProgramTests
    {
        [Test]
        public static void GetStartupInformation_Succeeds()
        {
            var result = Program.GetStartupInformation();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.GreaterThan(0));
        }
    }
}