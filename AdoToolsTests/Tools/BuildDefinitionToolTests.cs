using System;
using System.Collections.Generic;
using DevOpsTools.Tools;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DevOpsTools.UnitTests.Tools
{
    [TestFixture]
    public class BuildToolTests
    {
        [SetUp]
        public void SetUp()
        {
            _clientMock = TestHelper.SetUpMockClient();
            _fakeTool = new BuildDefinitionTool(
                _clientMock.Object, 
                TestHelper.RealOrganization,
                TestHelper.RealProject, 
                TestHelper.RealProjectId);
        }

        private BuildDefinitionTool _fakeTool;
        private Mock<IClient> _clientMock;

        [Test]
        public void GetMany_ReturnsExpectedResult()
        {
            var result = _fakeTool.GetMany().Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        public void Constructor_Succeeds()
        {
            var result =
                new BuildDefinitionTool(
                    _clientMock.Object,
                    TestHelper.RealOrganization,
                    TestHelper.RealProject,
                    TestHelper.RealProjectId
                );

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Copy_by_id_throws(
            [Values(1, -100, -1)] long id,
            [Values("new", null, "", " ")] string newName
        )
        {
            if (id < 0 || string.IsNullOrWhiteSpace(newName))
            {
                Assert.That(() => _fakeTool.Copy(id, newName), Throws.TypeOf<ArgumentException>());
            }
        }

        [Test]
        public void Copy_by_name_throws(
            [Values("old", null, "", " ")] string oldName,
            [Values("new", null, "", " ")] string newName
        )
        {
            if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
            {
                Assert.That(() => _fakeTool.Copy(oldName, newName), Throws.TypeOf<ArgumentException>());
            }
        }

        [Test]
        public void Create_throws(
            [Values(null, "", " ")] string definition
        )
        {
            Assert.That(() => _fakeTool.Create(definition), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Delete_by_id_throws(
            [Values(-100, -1)] long id
        )
        {
            Assert.That(() => _fakeTool.Delete(id), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Delete_by_name_throws(
            [Values(null, "", " ")] string name
        )
        {
            Assert.That(() => _fakeTool.Delete(name), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [Category("Integration")]
        public static void Get_by_id_returnsExpectedResult()
        {
            const long id = 1818;

            var realTool = MakeRealTool();

            var result = realTool.Get(id).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.GreaterThan(0));

            dynamic defDynamic = JsonConvert.DeserializeObject(result);

            Assert.That(defDynamic.id.Value, Is.EqualTo(id));

            Console.WriteLine(result);
        }

        [Test]
        public void Get_by_id_throws(
            [Values(-100, -1)] long id
        )
        {
            Assert.That(() => _fakeTool.Get(id), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [Category("Integration")]
        public static void Get_by_name_ReturnsExpectedResult()
        {
            const string name = "Microservice-Healthwise-DevBranch-CI";

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
        public static void Get_with_missing_id_Throws1()
        {
            var realTool = MakeRealTool();

            Assert.That(() =>
                    realTool.Get(long.MaxValue),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ClientException>());
        }

        [Test]
        [Category("Integration")]
        public static void GetId_ReturnsExpectedResult()
        {
            var realTool = MakeRealTool();

            var result = realTool.GetId("Microservice-Healthwise-DevBranch-CI").Result;

            Assert.That(result, Is.GreaterThanOrEqualTo(0));
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
        public void Rename_by_id_throws(
            [Values(1, -100, -1)] long id,
            [Values("new", null, "", " ")] string newName
        )
        {
            if (id < 0 || string.IsNullOrWhiteSpace(newName))
            {
                Assert.That(() => _fakeTool.Rename(id, newName), Throws.TypeOf<ArgumentException>());
            }
        }

        [Test]
        public void Rename_by_name_throws(
            [Values("old", null, "", " ")] string oldName,
            [Values("new", null, "", " ")] string newName
        )
        {
            if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
            {
                Assert.That(() => _fakeTool.Rename(oldName, newName), Throws.TypeOf<ArgumentException>());
            }
        }

        [Test]
        public void Update_throws(
            [Values(null, "", " ")] string definition
        )
        {
            Assert.That(() => _fakeTool.Update(definition), Throws.TypeOf<ArgumentException>());
        }

        private static BuildDefinitionTool MakeRealTool()
        {
            var pat = ToolHelper.GetPersonalAccessToken();
            var client = new Client(pat);
            return new BuildDefinitionTool(
                client, 
                TestHelper.RealOrganization, 
                TestHelper.RealProject,
                TestHelper.RealProjectId);
        }
    }
}