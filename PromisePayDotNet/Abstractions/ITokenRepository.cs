using PromisePayDotNet.DTO;
using System.Collections.Generic;

namespace PromisePayDotNet.Abstractions
{
    public interface ITokenRepository
    {
        string RequestToken();

        IDictionary<string, object> RequestSessionToken(Token token);

        Widget GetWidget(string sessionToken);

        CardToken GenerateCardToken(string tokenType, string userId);
    }
}
