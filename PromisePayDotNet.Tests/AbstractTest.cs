using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PromisePayDotNet.Internals;
using PromisePayDotNet.Settings;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PromisePayDotNet.Tests
{
    public abstract class AbstractTest
    {
        private static IServiceProvider CreateDi(IRestClient client = null)
        {
            var services = new ServiceCollection();
            if (null != client)
            {
                services.AddTransient(ci => client);
            }
            services.AddPromisePay(new PromisePaySettings
            {
                ApiUrl = "https://test.api.promisepay.com",
                Login = "idsidorov@gmail.com",
                Password = "mJrUGo2Vxuo9zqMVAvkw"
            });
            if (null != client)
            {
                services.AddTransient(ci => client);
            }
            services.AddOptions();
            services.AddLogging();
            return services.BuildServiceProvider();
        }


        protected TRepo Get<TRepo>() => CreateDi().GetRequiredService<TRepo>();
        protected TRepo Get<TRepo>(IRestClient client) => CreateDi(client).GetRequiredService<TRepo>();

        protected Mock<IRestClient> GetMockClient(string content)
        {
            return GetMockClient(content, HttpStatusCode.OK);
        }

        protected Mock<IRestClient> GetMockClient(string content, HttpStatusCode StatusCode)
        {
            var response = new Mock<RestResponse>(MockBehavior.Strict);
            response.SetupGet(x => x.Content).Returns(content);
            response.SetupGet(x => x.ResponseUri).Returns(new Uri("http://google.com"));
            response.SetupGet(x => x.StatusDescription).Returns("");
            response.SetupGet(x => x.StatusCode).Returns(StatusCode);

            var client = new Mock<IRestClient>(MockBehavior.Strict);
            client.SetupSet(x => x.BaseUrl = It.IsAny<Uri>());
            client.SetupSet(x => x.Authenticator = It.IsAny<IAuthenticator>());
            client.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>())).ReturnsAsync(response.Object);
            return client;
        }

    }
}
