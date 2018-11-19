using PromisePayDotNet.Implementations;
using PromisePayDotNet.Interfaces;
using Dynamic = PromisePayDotNet.Dynamic;
using RestSharp;
using Microsoft.Extensions.DependencyInjection;
using PromisePayDotNet.Settings;

namespace Microsoft.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAssemblyPay(this IServiceCollection serviceCollection, ISettings settings)
        {
            serviceCollection
                .AddSingleton<IRestClient,RestClient>()
                .AddSingleton<ISettings>(settings)

            //Old style repositories
                .AddScoped<IAddressRepository, AddressRepository>()
                .AddScoped<IBankAccountRepository, BankAccountRepository>()
                .AddScoped<ICardAccountRepository, CardAccountRepository>()
                .AddScoped<ICompanyRepository, CompanyRepository>()
                .AddScoped<IFeeRepository, FeeRepository>()
                .AddScoped<IItemRepository, ItemRepository>()
                .AddScoped<IPayPalAccountRepository, PayPalAccountRepository>()
                .AddScoped<ITokenRepository, TokenRepository>()
                .AddScoped<ITransactionRepository, TransactionRepository>()
                .AddScoped<IUploadRepository, UploadRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                ;
            return serviceCollection;
        }

        public static IServiceCollection AddAssemblyPayDynamic(this ServiceCollection serviceCollection, ISettings settings)
        {
            serviceCollection
                .AddSingleton<IRestClient,RestClient>()
                .AddSingleton<ISettings>(settings)

                //Dynamic repositories
                .AddScoped<Dynamic.Interfaces.IAddressRepository, Dynamic.Implementations.AddressRepository>()
                .AddScoped<Dynamic.Interfaces.IBankAccountRepository, Dynamic.Implementations.BankAccountRepository>()
                .AddScoped<Dynamic.Interfaces.IBatchTransactionRepository, Dynamic.Implementations.BatchTransactionRepository>()
                .AddScoped<Dynamic.Interfaces.ICardAccountRepository, Dynamic.Implementations.CardAccountRepository>()
                .AddScoped<Dynamic.Interfaces.IChargeRepository, Dynamic.Implementations.ChargeRepository>()
                .AddScoped<Dynamic.Interfaces.ICompanyRepository, Dynamic.Implementations.CompanyRepository>()
                .AddScoped<Dynamic.Interfaces.IConfigurationRepository, Dynamic.Implementations.ConfigurationRepository>()
                .AddScoped<Dynamic.Interfaces.IDirectDebitAuthorityRepository, Dynamic.Implementations.DirectDebitAuthorityRepository>()
                .AddScoped<Dynamic.Interfaces.IFeeRepository, Dynamic.Implementations.FeeRepository>()
                .AddScoped<Dynamic.Interfaces.IItemRepository, Dynamic.Implementations.ItemRepository>()
                .AddScoped<Dynamic.Interfaces.IMarketplaceRepository, Dynamic.Implementations.MarketplaceRepository>()
                .AddScoped<Dynamic.Interfaces.IPayPalAccountRepository, Dynamic.Implementations.PayPalAccountRepository>()
                .AddScoped<Dynamic.Interfaces.IRestrictionRepository, Dynamic.Implementations.RestrictionRepository>()
                .AddScoped<Dynamic.Interfaces.ITokenRepository, Dynamic.Implementations.TokenRepository>()
                .AddScoped<Dynamic.Interfaces.IToolRepository, Dynamic.Implementations.ToolRepository>()
                .AddScoped<Dynamic.Interfaces.ITransactionRepository, Dynamic.Implementations.TransactionRepository>()
                .AddScoped<Dynamic.Interfaces.IUploadRepository, Dynamic.Implementations.UploadRepository>()
                .AddScoped<Dynamic.Interfaces.IUserRepository, Dynamic.Implementations.UserRepository>()
                .AddScoped<Dynamic.Interfaces.IWalletRepository, Dynamic.Implementations.WalletRepository>()
            ;
            return serviceCollection;
        }
    }
}