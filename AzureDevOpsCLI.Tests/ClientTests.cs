using System;
using DevOpsTools;
using NUnit.Framework;

namespace VSTSTool.UnitTests
{
    [TestFixture]
    public static class ClientTests
    {
        [Test]
        public static void AddHeaders_Succeeds()
        {
            var result = new Client("fakePat");

            Assert.That(result, Is.Not.Null);

            result.AddHeaders();
        }

        [Test]
        public static void Constructor_Succeeds()
        {
            var result = new Client("fakePat");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public static void Constructor_Throws([Values(null, "", " ")] string pat)
        {
            Assert.That(() => new Client(pat), Throws.ArgumentException);
        }

        [Test]
        public static void Delete_Throws()
        {
            var client = new Client("fakePat");

            Assert.That(() => client.DeleteAsync(new Uri("https://www.google.com")).Result, Throws.Exception);
        }

        [Test]
        public static void Get_Succeeds()
        {
            var client = new Client("fakePat");

            var result = client.GetStringAsync(new Uri("https://www.google.com")).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        public static void Post_Throws()
        {
            var client = new Client("fakePat");

            Assert.That(() => client.PostStringAsync(new Uri("https://www.google.com"), "xxx").Result,
                Throws.Exception);
        }

        [Test]
        public static void Put_Succeeds()
        {
            var client = new Client("fakePat");

            Assert.That(() => client.PutStringAsync(new Uri("https://www.google.com"), "xxx").Result,
                Throws.Exception);
        }
    }
}