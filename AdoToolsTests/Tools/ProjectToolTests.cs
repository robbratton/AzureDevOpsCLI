using System;
using System.Collections.Generic;
using DevOpsTools.Tools;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DevOpsTools.UnitTests.Tools
{
    [TestFixture]
    public class ProjectToolTests
    {
        [SetUp]
        public void SetUp()
        {
            _clientMock = TestHelper.SetUpMockClient();
            _fakeTool = new ProjectTool(_clientMock.Object, TestHelper.RealOrganization, TestHelper.RealProject);
        }

        private ProjectTool _fakeTool;
        private Mock<IClient> _clientMock;

        [Test]
        public void Constructor_Succeeds()
        {
            var result =
                new ProjectTool(
                    _clientMock.Object,
                    TestHelper.RealOrganization,
                    TestHelper.RealProject
                );

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Create_throws(
            [Values(null, "", " ")] string input)
        {
            Assert.That(() => _fakeTool.Create(input), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [Category("Integration")]
        public static void Get_by_id_returnsExpectedResult()
        {
            var id = new Guid("f39ff6aa-3b5f-41f1-ba61-e4a72a4a0d13");

            var realTool = MakeRealTool();

            var result = realTool.Get(id).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.GreaterThan(0));

            dynamic defDynamic = JsonConvert.DeserializeObject(result);

            Assert.That(defDynamic.id.Value, Is.EqualTo(id.ToString()));

            Console.WriteLine(result);
        }

        [Test]
        public void Get_by_id_throws(
        )
        {
            Assert.That(() => _fakeTool.Get(Guid.Empty), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [Category("Integration")]
        public static void Get_by_name_ReturnsExpectedResult()
        {
            const string name = "Apollo";

            var realTool = MakeRealTool();
            var result = realTool.Get(name).Result;

            Assert.That(result, Is.Not.Null);
            StringAssert.Contains(name, result);
        }

        [Test]
        public void Get_by_name_throws(
            [Values(null, "", " ")] string name
        )
        {
            Assert.That(() => _fakeTool.Get(name), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [Category("Integration")]
        public static void GetId_ReturnsExpectedResult()
        {
            var realTool = MakeRealTool();
            var result = realTool.GetId("Apollo");

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void GetId_throws(
            [Values(null, "", " ")] string name
        )
        {
            Assert.That(() => _fakeTool.GetId(name), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [Category("Integration")]
        public static void GetId_with_bad_name_Throws()
        {
            var realTool = MakeRealTool();
            Assert.That(() => realTool.GetId("ABC123"), Throws.TypeOf<KeyNotFoundException>());
        }

        [Test]
        [Category("Integration")]
        public static void GetMany_returnsExpectedResult()
        {
            var realTool = MakeRealTool();
            var result = realTool.GetMany().Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        public void CopyByName_throws()
        {
            Assert.That(() => _fakeTool.Copy("old", "new"), Throws.TypeOf<NotImplementedException>());
        }

        [Test]
        public void CopyById_throws()
        {
            Assert.That(() => _fakeTool.Copy(1, "new"), Throws.TypeOf<NotImplementedException>());
        }

        private static ProjectTool MakeRealTool()
        {
            var pat = ToolHelper.GetPersonalAccessToken();
            var client = new Client(pat);
            return new ProjectTool(client, TestHelper.RealOrganization, TestHelper.RealProject);
        }
    }
}