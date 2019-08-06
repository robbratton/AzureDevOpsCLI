using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace VSTSTool.UnitTests
{
    [TestFixture]
    public static class HelperTests
    {
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
        private class TestObject
        {
            // ReSharper disable UnusedMember.Global
            public string Field1 { get; set; } = "a";
            public int Field2 { get; set; } = 4;
            public string HiddenField { get; set; } = "Do Not Show";

            public string[] ArrayField { get; set; } = {"Value1", "Value2"};
            // ReSharper restore UnusedMember.Global
        }

        [Test]
        public static void DumpProperties_Succeeds()
        {
            var testObject = new TestObject();
            var result = Helper.DumpProperties(testObject, new List<string> {nameof(TestObject.HiddenField)});

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.GreaterThan(0));
            Assert.Multiple(() =>
                {
                    Assert.That(result.Contains(nameof(TestObject.HiddenField) + ": (masked)"), Is.True);
                    Assert.That(result.Contains(testObject.HiddenField), Is.False);
                }
            );
        }

        [TestCase(-1, "")]
        [TestCase(0, "")]
        [TestCase(1, " ")]
        [TestCase(2, "  ")]
        public static void MakeIndentString_ReturnsExpectedResult(int indent, string expectedOutput)
        {
            var result = Helper.MakeIndentString(indent);

            Assert.That(result.ToString(), Is.EqualTo(expectedOutput));
        }

    }
}