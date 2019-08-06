using System;
using NUnit.Framework;

namespace DevOpsTools.UnitTests
{
    [TestFixture]
    public class ValidatorTests
    {
        [Test]
        public static void ValidateId_Throws()
        {
            var validators = new Validators();
            Assert.That(() => validators.ValidateId(""), Throws.InvalidOperationException);
            Assert.That(() => validators.ValidateId(Guid.Empty), Throws.ArgumentException);
            Assert.That(() => validators.ValidateId((long)-4), Throws.ArgumentException);
        }

        [Test]
        public static void ValidateId_Succeeds()
        {
            var validators = new Validators();
            Assert.That(() => validators.ValidateId(Guid.NewGuid()), Throws.Nothing);
            Assert.That(() => validators.ValidateId((long) 123), Throws.Nothing);
        }
    }
}