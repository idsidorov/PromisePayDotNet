using PromisePayDotNet.Implementations;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.Internals;
using PromisePayDotNet.Settings;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions to add promise pay repositories
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add promise pay repositories
        /// </summary>
        public static IServiceCollection AddPromisePay(this IServiceCollection container)
        {
            return AddPromisePay(container, null);
        }
        /// <summary>
        /// Add promise pay repositories
        /// </summary>
        public static IServiceCollection AddPromisePay(this IServiceCollection container, PromisePaySettings options)
        {
            if (options != null) container.AddSingleton(Options.Options.Create(options));
            container.AddTransient<IRestClient>(c => new RestClient());
            container.AddTransient<IAddressRepository, AddressRepository>();
            container.AddTransient<IBankAccountRepository, BankAccountRepository>();
            container.AddTransient<ICardAccountRepository, CardAccountRepository>();
            container.AddTransient<ICompanyRepository, CompanyRepository>();
            container.AddTransient<IFeeRepository, FeeRepository>();
            container.AddTransient<IItemRepository, ItemRepository>();
            container.AddTransient<IPayPalAccountRepository, PayPalAccountRepository>();
            container.AddTransient<ITokenRepository, TokenRepository>();
            container.AddTransient<ITransactionRepository, TransactionRepository>();
            container.AddTransient<IUploadRepository, UploadRepository>();
            container.AddTransient<IUserRepository, UserRepository>();
            return container;
        }
    }
}
