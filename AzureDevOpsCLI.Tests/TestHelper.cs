using System;
using System.Threading.Tasks;
using DevOpsTools.Tools.Interfaces;
using Moq;

namespace VSTSTool.UnitTests
{
    public static class TestHelper
    {
        public static Mock<IBuildDefinitionTool> SetUpMockBuildTool()
        {
            var output = new Mock<IBuildDefinitionTool>();
            output.Setup(x => x.GetMany()).Returns(Task.FromResult(
                @"{ ""value"": [ {""name"": ""my name"", ""id"": ""123"" } ] }")
            );

            output.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult("{\"Get By Name\" : \"value\" }"));
            output.Setup(x => x.Get(It.IsAny<long>())).Returns(Task.FromResult("{\"Get By ID\" : \"value\" }"));
            output.Setup(x => x.Create(It.IsAny<string>()));
            output.Setup(x => x.Delete(It.IsAny<string>()));
            output.Setup(x => x.Copy(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult("{\"Copy\" : \"value\" }"));
            output.Setup(x => x.Rename(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult("{\"Rename\" : \"value\" }"));

            return output;
        }

        public static Mock<IReleaseDefinitionTool> SetUpMockReleaseTool()
        {
            var output = new Mock<IReleaseDefinitionTool>();
            output.Setup(x => x.GetMany()).Returns(Task.FromResult(
                @"{ ""value"": [ {""name"": ""my name"", ""id"": ""123"" } ] }")
            );

            output.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult("{\"Get By Name\" : \"value\" }"));
            output.Setup(x => x.Get(It.IsAny<long>())).Returns(Task.FromResult("{\"Get By ID\" : \"value\" }"));
            output.Setup(x => x.Create(It.IsAny<string>()));
            output.Setup(x => x.Delete(It.IsAny<string>()));
            output.Setup(x => x.Copy(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult("{\"Copy\" : \"value\" }"));
            output.Setup(x => x.Rename(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult("{\"Rename\" : \"value\" }"));

            return output;
        }

        public static Mock<IProjectTool> SetUpMockProjectTool()
        {
            var output = new Mock<IProjectTool>();
            output.Setup(x => x.GetMany()).Returns(Task.FromResult(
                @"{ ""value"": [ {""name"": ""my name"", ""id"": ""123"" } ] }")
            );

            output.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult("{\"Get By Name\" : \"value\" }"));
            output.Setup(x => x.Get(It.IsAny<Guid>())).Returns(Task.FromResult("{\"Get By ID\" : \"value\" }"));
            output.Setup(x => x.Create(It.IsAny<string>()));
            output.Setup(x => x.Delete(It.IsAny<string>()));
            output.Setup(x => x.Copy(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult("{\"Copy\" : \"value\" }"));
            output.Setup(x => x.Rename(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult("{\"Rename\" : \"value\" }"));

            return output;
        }

        public static Mock<IRepositoryTool> SetUpMockRepositoryTool()
        {
            var output = new Mock<IRepositoryTool>();
            output.Setup(x => x.GetMany()).Returns(
                Task.FromResult(@"{ ""value"": [ {""name"": ""my name"", ""id"": ""123"" } ] }")
            );

            output.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult("{\"Get By Name\" : \"value\" }"));
            output.Setup(x => x.Get(It.IsAny<Guid>())).Returns(Task.FromResult("{\"Get By ID\" : \"value\" }"));
            output.Setup(x => x.Create(It.IsAny<string>()));
            output.Setup(x => x.Delete(It.IsAny<string>()));
            output.Setup(x => x.Copy(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult("{\"Copy\" : \"value\" }"));
            output.Setup(x => x.Rename(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult("{\"Rename\" : \"value\" }"));

            return output;
        }

        public static Mock<ITaskGroupTool> SetUpMockTaskTool()
        {
            var output = new Mock<ITaskGroupTool>();
            output.Setup(x => x.GetMany()).Returns(Task.FromResult(
                @"{ ""value"": [ {""name"": ""my name"", ""id"": ""xxx"" } ] }")
            );
            output.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult("{\"Get By Name\" : \"value\" }"));
            output.Setup(x => x.Get(It.IsAny<Guid>())).Returns(Task.FromResult("{\"Get By ID\" : \"value\" }"));
            output.Setup(x => x.Create(It.IsAny<string>()));
            output.Setup(x => x.Delete(It.IsAny<string>()));
            output.Setup(x => x.Copy(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult("{\"Copy\" : \"value\" }"));
            output.Setup(x => x.Rename(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult(
                    "{\"Rename\" : \"value\" }"));

            return output;
        }

        public static Mock<IVariableGroupTool> SetUpMockVariableTool()
        {
            var output = new Mock<IVariableGroupTool>();
            output.Setup(x => x.GetMany()).Returns(Task.FromResult(
                @"{ ""value"": [ {""name"": ""my name"", ""id"": ""123"" } ] }")
            );

            output.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult("{\"Get By Name\" : \"value\" }"));
            output.Setup(x => x.Get(It.IsAny<long>())).Returns(Task.FromResult("{\"Get By ID\" : \"value\" }"));
            output.Setup(x => x.Create(It.IsAny<string>()));
            output.Setup(x => x.Delete(It.IsAny<string>()));
            output.Setup(x => x.Copy(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult("{\"Copy\" : \"value\" }"));
            output.Setup(x => x.Rename(It.IsAny<string>(), It.IsAny<string>())).Returns(
                Task.FromResult("{\"Rename\" : \"value\" }"));

            return output;
        }
    }
}