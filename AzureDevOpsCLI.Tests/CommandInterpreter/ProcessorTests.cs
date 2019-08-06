using System;
using System.Collections.Generic;
using DevOpsTools.Tools.Interfaces;
using Moq;
using NUnit.Framework;
using VSTSTool.CommandInterpreter;

namespace VSTSTool.UnitTests.CommandInterpreter
{
    [TestFixture]
    public class ProcessorTests
    {
        [SetUp]
        public void SetUp()
        {
            _commandsMock = new Mock<ICommands>();

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

        private Mock<ICommands> _commandsMock;
        private ICommands _commands;

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Process_Throws(
            bool includeArgs,
            bool includeCommands)
        {
            var args = includeArgs ? Array.Empty<string>() : null;
            var commands = includeCommands ? _commandsMock.Object : null;

            Assert.That(() => Processor.Process(args, commands), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Process_CallsExpectedMethods(
        [Values] Command command,
        [Values] ItemType itemType)
        {
            var args = SetUpArguments(command, itemType);

            switch (command)
            {
                case Command.CreateFork when itemType != ItemType.Repository:
                    {
                        // This will have errorLevel 1 because calling CreateFork on anything but Repository throws an exception. It will trigger the warnings tab in ReSharper's Unit Test Sessions window.
                        var errorLevel = Processor.Process(args.ToArray(), _commands);
                        Assert.That(errorLevel, Is.EqualTo(1), "ErrorLevel");
                        break;
                    }
                case Command.Create:
                    {
                        // This will have errorLevel 1 because calling Create with a bad path will cause an exception. It will trigger the warnings tab in ReSharper's Unit Test Sessions window.
                        var errorLevel = Processor.Process(args.ToArray(), _commands);
                        Assert.That(errorLevel, Is.EqualTo(1), "ErrorLevel");
                        break;
                    }
                case Command.Copy:
                case Command.CreateFork:
                case Command.Delete:
                case Command.Dump:
                case Command.List:
                case Command.Rename:
                    {
                        // var errorLevel =
                        Processor.Process(args.ToArray(), _commands);

                        // todo This sometimes is 1 and sometimes is zero.
                        // Assert.That(errorLevel, Is.EqualTo(1), "ErrorLevel");

                        Assert.Multiple(() =>
                        {
                            switch (itemType)
                            {
                                case ItemType.BuildDefinition:
                                    VerifyBuildDefinitionCalls(command);
                                    break;

                                case ItemType.ReleaseDefinition:
                                    VerifyReleaseDefinitionCalls(command);
                                    break;

                                case ItemType.Project:
                                    VerifyProjectCalls(command);
                                    break;

                                case ItemType.TaskGroup:
                                    VerifyTaskGroupCalls(command);
                                    break;

                                case ItemType.VariableGroup:
                                    VerifyVariableGroupCalls(command);
                                    break;

                                case ItemType.Repository:
                                    VerifyRepositoryCalls(command);
                                    break;

                                default:
                                    Assert.Fail($"Unexpected Item {itemType}");
                                    break;
                            }
                        });
                    }
                    break;

                default:
                    Assert.Fail($"Unexpected command {command}");
                    break;
            }
        }

        private void VerifyRepositoryCalls(Command command)
        {
            switch (command)
            {
                case Command.Create:
                    _taskToolMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
                    break;
                case Command.List:
                    _repositoryToolMock.Verify(x => x.GetMany(), Times.Once);
                    break;
                case Command.Dump:
                    _repositoryToolMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Delete:
                    _repositoryToolMock.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Copy:
                    _repositoryToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                case Command.Rename:
                    _repositoryToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                case Command.CreateFork:
                    _repositoryToolMock.Verify(x => x.CreateFork(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                default:
                    Assert.Fail($"Unexpected command {command}");
                    break;
            }
        }

        private void VerifyVariableGroupCalls(Command command)
        {
            switch (command)
            {
                case Command.Create:
                    _taskToolMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
                    break;
                case Command.List:
                    _variableToolMock.Verify(x => x.GetMany(), Times.Once);
                    break;
                case Command.Dump:
                    _variableToolMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Delete:
                    _variableToolMock.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Copy:
                    _variableToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                case Command.Rename:
                    _variableToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                case Command.CreateFork:
                    // Ignore. Only applies to Repository items.
                    break;
                default:
                    Assert.Fail($"Unexpected command {command}");
                    break;
            }
        }

        private void VerifyTaskGroupCalls(Command command)
        {
            switch (command)
            {
                case Command.Create:
                    _taskToolMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
                    break;
                case Command.List:
                    _taskToolMock.Verify(x => x.GetMany(), Times.Once);
                    break;
                case Command.Dump:
                    _taskToolMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Delete:
                    _taskToolMock.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Copy:
                    _taskToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                    break;
                case Command.Rename:
                    _taskToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                    break;
                case Command.CreateFork:
                    // Ignore. Only applies to Repository items.
                    break;
                default:
                    Assert.Fail($"Unexpected command {command}");
                    break;
            }
        }

        private void VerifyBuildDefinitionCalls(Command command)
        {
            switch (command)
            {
                case Command.Create:
                    _buildToolMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Copy:
                    _buildToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                    break;
                case Command.CreateFork:
                    // Ignore. Only applies to Repository items.
                    break;
                case Command.Delete:
                    _buildToolMock.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Dump:
                    _buildToolMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
                    break;
                case Command.List:
                    _buildToolMock.Verify(x => x.GetMany(), Times.Once);
                    break;
                case Command.Rename:
                    _buildToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                default:
                    Assert.Fail($"Unexpected command {command}");
                    break;
            }
        }

        private void VerifyReleaseDefinitionCalls(Command command)
        {
            switch (command)
            {
                case Command.Create:
                    _releaseToolMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Copy:
                    _releaseToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                    break;
                case Command.CreateFork:
                    // Ignore. Only applies to Repository items.
                    break;
                case Command.Delete:
                    _releaseToolMock.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Dump:
                    _releaseToolMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
                    break;
                case Command.List:
                    _releaseToolMock.Verify(x => x.GetMany(), Times.Once);
                    break;
                case Command.Rename:
                    _releaseToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                default:
                    Assert.Fail($"Unexpected command {command}");
                    break;
            }
        }

        private void VerifyProjectCalls(Command command)
        {
            switch (command)
            {
                case Command.Create:
                    _taskToolMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
                    break;
                case Command.List:
                    _projectToolMock.Verify(x => x.GetMany(), Times.Once);
                    break;
                case Command.Dump:
                    _projectToolMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Delete:
                    _projectToolMock.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
                    break;
                case Command.Copy:
                    _projectToolMock.Verify(x => x.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                    break;
                case Command.Rename:
                    _projectToolMock.Verify(x => x.Rename(It.IsAny<string>(), It.IsAny<string>()),
                        Times.Once);
                    break;
                case Command.CreateFork:
                    // Ignore. Only applies to Repository items.
                    break;
                default:
                    Assert.Fail($"Unexpected command {command}");
                    break;
            }
        }

        private static List<string> SetUpArguments(Command command, ItemType item)
        {
            var args = new List<string> { command.ToString(), item.ToString() };

            switch (command)
            {
                case Command.Dump:
                case Command.Delete:
                    args.Add("name");
                    break;
                case Command.Create:
                    args.Add("content");
                    break;
                case Command.Copy:
                case Command.CreateFork:
                case Command.Rename:
                    args.AddRange(new[] { "oldName", "newName" });
                    break;
                case Command.List:
                    args.Add("service");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }

            return args;
        }
    }
}