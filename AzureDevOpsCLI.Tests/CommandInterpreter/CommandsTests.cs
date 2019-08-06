using System;
using DevOpsTools.Tools.Interfaces;
using Moq;
using NUnit.Framework;
using VSTSTool.CommandInterpreter;

namespace VSTSTool.UnitTests.CommandInterpreter
{
    [TestFixture]
    public class CommandsTests
    {
        [SetUp]
        public void SetUp()
        {
            _buildToolMock = TestHelper.SetUpMockBuildTool();
            _releaseToolMock = TestHelper.SetUpMockReleaseTool();
            _projectToolMock = TestHelper.SetUpMockProjectTool();
            _repositoryToolMock = TestHelper.SetUpMockRepositoryTool();
            _taskToolMock = TestHelper.SetUpMockTaskTool();
            _variableToolMock = TestHelper.SetUpMockVariableTool();

            _commands = new Commands(
                _buildToolMock.Object,
                _releaseToolMock.Object,
                _projectToolMock.Object,
                _repositoryToolMock.Object,
                _taskToolMock.Object,
                _variableToolMock.Object);
        }

        private Mock<IBuildDefinitionTool> _buildToolMock;
        private Mock<IReleaseDefinitionTool> _releaseToolMock;
        private Mock<IProjectTool> _projectToolMock;
        private Mock<IRepositoryTool> _repositoryToolMock;
        private Mock<ITaskGroupTool> _taskToolMock;
        private Mock<IVariableGroupTool> _variableToolMock;

        private ICommands _commands;

        [Test]
        public void Constructor_throws(
            [Values] bool provideBuild,
            [Values] bool provideRelease,
            [Values] bool provideProject,
            [Values] bool provideRepository,
            [Values] bool provideTask,
            [Values] bool provideVariable
        )
        {
            var buildTool = provideBuild ? _buildToolMock.Object : null;
            var releaseTool = provideRelease ? _releaseToolMock.Object : null;
            var projectTool = provideProject ? _projectToolMock.Object : null;
            var repositoryTool = provideRepository ? _repositoryToolMock.Object : null;
            var taskTool = provideTask ? _taskToolMock.Object : null;
            var variableTool = provideVariable ? _variableToolMock.Object : null;

            if (!provideBuild || !provideRelease || !provideProject || !provideRepository || !provideTask || !provideVariable)
            {
                Assert.That(() => new Commands(
                        buildTool,
                        releaseTool,
                        projectTool,
                        repositoryTool,
                        taskTool,
                        variableTool),
                    Throws.ArgumentException);
            }
            else
            {
                Assert.That(() => new Commands(
                    buildTool,
                    releaseTool,
                    projectTool,
                    repositoryTool,
                    taskTool,
                    variableTool), Throws.Nothing);
            }
        }

        [Test]
        public void Copy_Succeeds(
            [Values] ItemType itemType
        )
        {
            switch (itemType)
            {
                case ItemType.BuildDefinition:
                    Assert.That(() => _commands.Copy(itemType, "name1", "name2"), Throws.Nothing);
                    _buildToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                case ItemType.ReleaseDefinition:
                    Assert.That(() => _commands.Copy(itemType, "name1", "name2"), Throws.Nothing);
                    _releaseToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                case ItemType.Repository:
                    Assert.That(() => _commands.Copy(itemType, "name1", "name2"), Throws.Nothing);
                    _repositoryToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                case ItemType.TaskGroup:
                    Assert.That(() => _commands.Copy(itemType, "name1", "name2"), Throws.Nothing);
                    _taskToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                case ItemType.VariableGroup:
                    Assert.That(() => _commands.Copy(itemType, "name1", "name2"), Throws.Nothing);
                    _variableToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                case ItemType.Project:
                    Assert.That(() => _commands.Copy(itemType, "name1", "name2"), Throws.Nothing);
                    _projectToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                default:
                    Assert.Fail($"Unexpected Item {itemType}");
                    break;
            }
        }

        [Test]
        public void Copy_throws(
            [Values] ItemType itemType,
            [Values("OLD", null, "", " ")] string oldName,
            [Values("new", null, "", " ")] string newName
        )
        {
            {
                if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
                {
                    Assert.That(() =>
                            _commands.Copy(
                                itemType,
                                oldName,
                                newName
                            ),
                        Throws.ArgumentException);
                }
            }
        }

        [Test]
        public void CreateFork_throws(
            [Values] ItemType itemType,
            [Values("OLD", null, "", " ")] string oldName,
            [Values("new", null, "", " ")] string newName
        )
        {
            if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
            {
                Assert.That(() =>
                        _commands.CreateFork(
                            itemType,
                            oldName,
                            newName
                        ),
                    Throws.ArgumentException);
            }
        }

        [Test]
        public void Create_throws(
            [Values] ItemType itemType,
            [Values(null, "", " ")] string name
        )
        {
            Assert.That(() => _commands.Create(itemType, name), Throws.ArgumentException);
        }

        [Test]
        public void Delete_throws(
            [Values] ItemType itemType,
            [Values(null, "", " ")] string name
        )
        {
            Assert.That(() => _commands.Delete(itemType, name), Throws.ArgumentException);
        }

        [Test]
        public void Dump_Succeeds(
            [Values] ItemType itemType
        )
        {
            Assert.Multiple(() =>
            {
                Assert.That(() => _commands.Dump(itemType, "name"), Throws.Nothing);

                switch (itemType)
                {
                    case ItemType.BuildDefinition:
                        _buildToolMock.Verify(x => x.Get(It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.ReleaseDefinition:
                        _releaseToolMock.Verify(x => x.Get(It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.Project:
                        _projectToolMock.Verify(x => x.Get(It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.Repository:
                        _repositoryToolMock.Verify(x => x.Get(It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.TaskGroup:
                        _taskToolMock.Verify(x => x.Get(It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.VariableGroup:
                        _variableToolMock.Verify(x => x.Get(It.IsAny<string>()),
                            Times.Once);
                        break;
                    default:
                        Assert.Fail($"Unexpected Item {itemType}");
                        break;
                }
            });
        }

        [Test]
        public void Dump_throws(
            [Values] ItemType itemType,
            [Values(null, "", " ")] string name
        )
        {
            Assert.That(() => _commands.Dump(itemType, name), Throws.ArgumentException);
        }

        [Test]
        public void List_Succeeds(
            [Values] ItemType itemType,
            [Values(null, "service")] string filter
        )
        {
            Assert.Multiple(() =>
            {
                Assert.That(() => _commands.List(itemType, filter), Throws.Nothing);

                switch (itemType)
                {
                    case ItemType.BuildDefinition:
                        _buildToolMock.Verify(x => x.GetMany(),
                            Times.Once);
                        break;
                    case ItemType.ReleaseDefinition:
                        _releaseToolMock.Verify(x => x.GetMany(),
                            Times.Once);
                        break;
                    case ItemType.Project:
                        _projectToolMock.Verify(x => x.GetMany(),
                            Times.Once);
                        break;
                    case ItemType.Repository:
                        _repositoryToolMock.Verify(x => x.GetMany(),
                            Times.Once);
                        break;
                    case ItemType.TaskGroup:
                        _taskToolMock.Verify(x => x.GetMany(),
                            Times.Once);
                        break;
                    case ItemType.VariableGroup:
                        _variableToolMock.Verify(x => x.GetMany(),
                            Times.Once);
                        break;
                    default:
                        Assert.Fail($"Unexpected Item {itemType}");
                        break;
                }
            });
        }

        [Test]
        public void Rename_Succeeds(
            [Values] ItemType itemType
        )
        {
            Assert.Multiple(() =>
            {
                Assert.That(() => _commands.Rename(itemType, "name1", "name2"), Throws.Nothing);

                switch (itemType)
                {
                    case ItemType.BuildDefinition:
                        _buildToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.ReleaseDefinition:
                        _releaseToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.Project:
                        //ignore
                        break;
                    case ItemType.Repository:
                        _repositoryToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.TaskGroup:
                        _taskToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.VariableGroup:
                        _variableToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                            Times.Once);
                        break;
                    default:
                        Assert.Fail($"Unexpected Item {itemType}");
                        break;
                }
            });
        }

        [Test]
        public void Rename_throws(
            [Values] ItemType itemType,
            [Values("old", null, "", " ")] string oldName,
            [Values("new", null, "", " ")] string newName
        )
        {
            if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
            {
                Assert.That(() => _commands.Rename(itemType, oldName, newName), Throws.ArgumentException);
            }
            else
            {
                Assert.That(() => _commands.Rename(itemType, oldName, newName), Throws.Nothing);
                switch (itemType)
                {
                    case ItemType.BuildDefinition:
                        _buildToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.ReleaseDefinition:
                        _releaseToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.Repository:
                        _repositoryToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.Project:
                        //ignore
                        break;
                    case ItemType.TaskGroup:
                        _taskToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                            Times.Once);
                        break;
                    case ItemType.VariableGroup:
                        _variableToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                            Times.Once);
                        break;
                    default:
                        Assert.Fail($"Unexpected Item {itemType}");
                        break;
                }
            }
        }

    }
}