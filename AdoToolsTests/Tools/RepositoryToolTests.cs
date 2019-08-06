using System;
using System.Collections.Generic;
using DevOpsTools.Tools;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DevOpsTools.UnitTests.Tools
{
    [TestFixture]
    public class RepositoryToolTests
    {
        [SetUp]
        public void SetUp()
        {
            _clientMock = TestHelper.SetUpMockClient();

            _fakeTool = new RepositoryTool(
                _clientMock.Object,
                TestHelper.RealOrganization,
                TestHelper.RealProject,
                TestHelper.FakeProjectId);
        }

        private RepositoryTool _fakeTool;
        private Mock<IClient> _clientMock;

        private static RepositoryTool SetUpRealTool()
        {
            var pat = ToolHelper.GetPersonalAccessToken();
            var client = new Client(pat);

            var projectTool = new ProjectTool(
                client,
                TestHelper.RealOrganization,
                TestHelper.RealProject);

            var projectId = Guid.Parse(projectTool.GetId(TestHelper.RealProject).Result.ToString());

            var output = new RepositoryTool(
                client,
                TestHelper.RealOrganization,
                TestHelper.RealProject,
                projectId);

            return output;
        }

        [Test]
        public void Constructor_Succeeds()
        {
            var result =
                new RepositoryTool(
                    _clientMock.Object,
                    TestHelper.RealOrganization,
                    TestHelper.RealProject,
                    TestHelper.FakeProjectId
                );

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Create_throws(
            [Values(null, "", " ")] string definition
        )
        {
            Assert.That(() => _fakeTool.Create(definition), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void CreateBranch_throws(
            [Values("repo", null, "", " ")] string repository,
            [Values("old", null, "", " ")] string oldName,
            [Values("new", null, "", " ")] string newName
        )
        {
            if (string.IsNullOrWhiteSpace(repository)
                || string.IsNullOrWhiteSpace(oldName)
                || string.IsNullOrWhiteSpace(newName)
            )
            {
                Assert.That(() => _fakeTool.CreateBranch(repository, oldName, newName),
                    Throws.TypeOf<ArgumentException>());
            }
        }

        [Test]
        public void CreateFork_by_id_throws(
            [Values] bool provideId,
            [Values("new", null, "", " ")] string newName
        )
        {
            var id = provideId ? Guid.NewGuid() : Guid.Empty;

            if (id == Guid.Empty || string.IsNullOrWhiteSpace(newName))
            {
                Assert.That(() => _fakeTool.CreateFork(id, newName), Throws.TypeOf<ArgumentException>());
            }
        }

        [Test]
        public void CreateFork_by_name_throws(
            [Values("old", null, "", " ")] string oldName,
            [Values("new", null, "", " ")] string newName
        )
        {
            if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
            {
                Assert.That(() => _fakeTool.CreateFork(oldName, newName), Throws.TypeOf<ArgumentException>());
            }
        }

        [Test]
        public void Delete_by_id_throws()
        {
            Assert.That(() => _fakeTool.Delete(Guid.Empty), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Delete_by_name_throws(
            [Values(null, "", " ")] string name
        )
        {
            Assert.That(() => _fakeTool.Delete(name), Throws.TypeOf<ArgumentException>());
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
            const string name = "Upmc.DevTools.Common";

            var realTool = SetUpRealTool();
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
        public static void Get_returnsExpectedResult()
        {
            var id = new Guid("93008a6f-63cd-482d-9177-98667baef7f5");

            var realTool = SetUpRealTool();
            var result = realTool.Get(id).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.GreaterThan(0));

            dynamic defDynamic = JsonConvert.DeserializeObject(result);

            Assert.That(defDynamic.id.Value, Is.EqualTo(id.ToString()));

            Console.WriteLine(result);
        }

        [Test]
        public void GetBranches_throws(
            [Values] bool provideId
        )
        {
            var repositoryId = provideId ? Guid.NewGuid() : Guid.Empty;

            if (!provideId)
            {
                Assert.That(() => _fakeTool.GetBranchesMany(repositoryId), Throws.TypeOf<ArgumentException>());
            }
        }

        [Test]
        [Category("Integration")]
        public static void GetBranchId_ReturnsExpectedResult(
            [Values("upmc.DevTools.common", "Monolith-HP")] string repo,
            [Values("develop", "master")] string branch
        )
        {
            var realTool = SetUpRealTool();

            var repoId = Guid.Parse(realTool.GetId(repo).Result.ToString());
            Console.WriteLine($"Repo {repo} Id: {repoId}");

            var branchName = $"refs/heads/{branch}";

            var result = realTool.GetBranchId(repoId, branchName).Result;

            Console.WriteLine($"Branch Id:{result}");

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo("0000000000000000000000000000000000000000"));
        }

        [Test]
        [Category("Integration")]
        public static void GetBranchId_bad_branch_ReturnsExpectedResult(
            [Values("Microservice-Healthwise", "Monolith-HP")]
            string repo,
            [Values("FakeBranch1", "FakeBranch2")] string branch
        )
        {
            var realTool = SetUpRealTool();

            var repoId = Guid.Parse(realTool.GetId(repo).Result.ToString());
            Console.WriteLine($"Repo {repo} Id: {repoId}");

            var branchName = $"refs/heads/{branch}";

            Assert.That(() => realTool.GetBranchId(repoId, branchName), Throws.TypeOf<KeyNotFoundException>());
        }

        [Test]
        public void GetBranchId_throws(
            [Values] bool provideId,
            [Values(null, "", " ")] string name
        )
        {
            var repositoryId = provideId ? Guid.NewGuid() : Guid.Empty;

            if (!provideId || string.IsNullOrWhiteSpace(name))
            {
                Assert.That(() => _fakeTool.GetBranchId(repositoryId, name), Throws.TypeOf<ArgumentException>());
            }
        }

        [Test]
        [Category("Integration")]
        public static void GetId_ReturnsExpectedResult()
        {
            var realTool = SetUpRealTool();
            var result = realTool.GetId("Microservice-Healthwise");

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
            var realTool = SetUpRealTool();
            Assert.That(() => realTool.GetId("ABC123"), Throws.TypeOf<KeyNotFoundException>());
        }

        [Test]
        [Category("Integration")]
        public static void GetMany_returnsExpectedResult()
        {
            var realTool = SetUpRealTool();
            var result = realTool.GetMany().Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.GreaterThan(0));

            // too long! Console.WriteLine(result);
        }

        [Test]
        public void Rename_by_id_throws(
            [Values] bool provideId,
            [Values("new", null, "", " ")] string newName
        )
        {
            var id = provideId ? Guid.NewGuid() : Guid.Empty;

            if (id == Guid.Empty || string.IsNullOrWhiteSpace(newName))
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
            [Values(null, "", " ")] string body
        )
        {
            Assert.That(() => _fakeTool.Update(body), Throws.TypeOf<ArgumentException>());
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
    }
}