using Moq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Tests
{
    public class AbstractTest
    {
        protected Mock<IRestClient> GetMockClient(string content)
        {
            return GetMockClient(content, HttpStatusCode.OK);
        }

        protected Mock<IRestClient> GetMockClient(string content, HttpStatusCode StatusCode)
        {
            var response = new Mock<IRestResponse>(MockBehavior.Strict);
            response.SetupGet(x => x.Content).Returns(content);
            response.SetupGet(x => x.ResponseUri).Returns(new Uri("http://google.com"));
            response.SetupGet(x => x.StatusDescription).Returns("");
            response.SetupGet(x => x.StatusCode).Returns(StatusCode);

            var client = new Mock<IRestClient>(MockBehavior.Strict);
            client.SetupSet(x => x.BaseUrl = It.IsAny<Uri>());
            client.SetupSet(x => x.Authenticator = It.IsAny<IAuthenticator>());
            client.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response.Object);
            return client;
        }

        protected Mock<ISettings> GetMockSettings()
        {
            var mock = new Mock<ISettings>();
            mock.Setup(m => m.Url).Returns("https://test.api.promisepay.com");
            mock.Setup(m => m.Login).Returns("idsidorov@gmail.com");
            mock.Setup(m => m.Password).Returns("mJrUGo2Vxuo9zqMVAvkw");
            return mock;
        }

        protected Mock<ILogger<T>> GetMockLogger<T>()
        {
            return new Mock<ILogger<T>>();
        }

    }
}
