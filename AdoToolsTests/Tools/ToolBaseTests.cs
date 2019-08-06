using System.Threading.Tasks;
using DevOpsTools.Tools;
using Moq;
using NUnit.Framework;

namespace DevOpsTools.UnitTests.Tools
{
    [TestFixture]
    public class ToolBaseTests
    {
        [Test]
        public static void Constructor_Validation(
            [Values] bool provideClient,
            [Values] bool provideProject,
            [Values] bool provideBasePath,
            [Values] bool provideApiVersionSuffix)
        {
            IClient client = null;
            if (provideClient)
            {
                client = new Mock<IClient>().Object;
            }
            string project = null;
            if (provideProject)
            {
                project = "Project";
            }
            string basePath = null;
            if (provideBasePath)
            {
                basePath = "BasePath";
            }

            string apiVersionSuffix = null;
            if (provideApiVersionSuffix)
            {
                apiVersionSuffix = "ApiVersionSuffix";
            }


            if (provideProject && provideApiVersionSuffix && provideBasePath && provideClient) {
                var result = new ToolBaseTester(client, project, basePath, apiVersionSuffix);
                Assert.That(result, Is.Not.Null);
            }
            else
            {
                Assert.That(() =>
                {
                    var _ = new ToolBaseTester(client, project, basePath, apiVersionSuffix);
                }, Throws.ArgumentNullException);
            }

            // todo Test base methods with mocks.
        }
    }

    internal class ToolBaseTester : ToolBase
    {
        public ToolBaseTester(IClient client, string project, string basePath, string apiVersionSuffix) : base(client, project, basePath, apiVersionSuffix)
        {
        }

        protected override Task CopyImpl(string oldName, string newName)
        {
            throw new System.NotImplementedException();
        }

        protected override Task CopyImpl(object id, string newName)
        {
            throw new System.NotImplementedException();
        }
    }
}
