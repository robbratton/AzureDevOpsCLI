using NUnit.Framework;

namespace DevOpsTools.UnitTests
{
    [TestFixture]
    public class ToolsTests
    {
        [TestCase("Microservice-XYZ-DevBranch-Checkmarx", "Microservice-XYZ")]
        [TestCase("Monolith-XYZ-DevBranch-Checkmarx", "Monolith-XYZ")]
        [TestCase("NuGet-Upmc.XYZ-DevBranch-Checkmarx", "Upmc.XYZ")]
        public static void GetRepoNameFromBuildName_ReturnsExpectedResult(string input, string expectedOutput)
        {
            var result = Helpers.GetRepoNameFromBuildName(input);

            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        [TestCase("Microservice-XYZ-DevBranch-Checkmarx", "WIP-RobBratton-Microservices", "AppServices-POC-Microservice-XYZ")]
        [TestCase("Monolith-XYZ-DevBranch-Checkmarx", "WIP-RobBratton-Monolith", "AppServices-POC-Monolith-XYZ")]
        [TestCase("NuGet-Upmc.XYZ-DevBranch-Checkmarx", "WIP-RobBratton-NuGet", "AppServices-POC-NuGet-Upmc.XYZ")]
        public static void GetCheckmarxPresetAndProject_ReturnsExpectedResult(string input, string expectedPreset, string expectedProject)
        {
            Helpers.GetCheckmarxPresetAndProject(input, out var preset, out var project);

            Assert.Multiple(() => { 
            Assert.That(preset, Is.EqualTo(expectedPreset));
            Assert.That(project, Is.EqualTo(expectedProject));
            });
        }

    }
}