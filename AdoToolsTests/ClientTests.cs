using System;
using NUnit.Framework;

namespace DevOpsTools.UnitTests
{
    [TestFixture]
    public static class ClientTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public static void Constructor_Throws(string personalAccessToken)
        {
            Assert.That(() =>
                    new Client(personalAccessToken),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public static void AddHeadersToClient_Succeeds()
        {
            var client = new Client("fakePat");
            Assert.That(() =>
                    client.AddHeaders(),
                Throws.Nothing);
        }
    }
}