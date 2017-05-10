using PromisePayDotNet.DTO;
using PromisePayDotNet.Internals;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromisePayDotNet.Abstractions
{
    public interface ITokenRepository
    {
        /// <summary>
        /// NOTE: Not documented on site
        /// </summary>
        Task<string> RequestTokenAsync();
        /// <summary>
        /// NOTE: Not documented on site
        /// </summary>
        Task<IDictionary<string, object>> RequestSessionTokenAsync(Token token);
        /// <summary>
        /// NOTE: Not documented on site
        /// </summary>
        Task<Widget> GetWidgetAsync(string sessionToken);
        /// <summary>
        /// https://reference.assemblypayments.com/#token-auth
        /// Create a card token that can be used with the PromisePay.js package to securely send Assembly credit card details.
        /// </summary>
        Task<CardToken> GenerateCardTokenAsync(string tokenType, string userId);
    }
    public static class TokenRepositoryExtensions
    {
        public static string RequestToken(this ITokenRepository repo) => repo.RequestTokenAsync().WrapResult();

        public static IDictionary<string, object> RequestSessionToken(this ITokenRepository repo,Token token) => repo.RequestSessionTokenAsync(token).WrapResult();

        public static Widget GetWidget(this ITokenRepository repo, string sessionToken) => repo.GetWidgetAsync(sessionToken).WrapResult();

        public static CardToken GenerateCardToken(this ITokenRepository repo, string tokenType, string userId) => repo.GenerateCardTokenAsync(tokenType, userId).WrapResult();
    }
}
