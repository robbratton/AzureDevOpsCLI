using System;
using NUnit.Framework;

namespace DevOpsTools.UnitTests
{
    [TestFixture]
    public static class ToolHelperTests
    {
        [Test]
        [Category("Integration")]
        public static void GetPersonalAccessToken_Succeeds()
        {
            ToolHelper.GetPersonalAccessToken();
        }
    }
}