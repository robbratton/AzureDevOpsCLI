using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;

// ReSharper disable UnusedMember.Global

namespace DevOpsTools.UnitTests
{
    public static class TestHelper
    {
        // ReSharper disable once InconsistentNaming
        public const string RealOrganization = "upmcappsvcs";
        // ReSharper disable once InconsistentNaming
        public const string RealProject = "Apollo";
        public static Guid FakeProjectId = Guid.NewGuid();
        public static Guid RealProjectId = Guid.Parse("f39ff6aa-3b5f-41f1-ba61-e4a72a4a0d13");

        public static Mock<IClient> SetUpMockClient()
        {
            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.AddHeaders());
            clientMock.Setup(x => x
                    .DeleteAsync(It.IsAny<Uri>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
            clientMock.Setup(x => x
                    .GetStringAsync(It.IsAny<Uri>()))
                .Returns(Task.FromResult("result"));
            clientMock.Setup(x => x
                    .PostStringAsync(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
            clientMock.Setup(x => x
                    .PutStringAsync(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
            return clientMock;
        }
    }
}