using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using PromisePayDotNet.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace PromisePayDotNet.Tests
{
    public class DITest
    {
        [Test]
        public void TestDIContainer()
        {
            // ARRANGE
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddSingleton<ILoggerFactory>(new LoggerFactory());
            serviceCollection.AddAssemblyPay(new PromisePayDotNet.Settings.Settings{
                Url = "https://test.api.promisepay.com",
                Login = "idsidorov@gmail.com",
                Password ="mJrUGo2Vxuo9zqMVAvkw"
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // ACT
            var userService = serviceProvider.GetService<IUserRepository>();
            Assert.IsNotNull(userService);
        }
    }
}
