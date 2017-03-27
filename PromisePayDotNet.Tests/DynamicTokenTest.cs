using Xunit;
using System.IO;
using PromisePayDotNet.Abstractions;

namespace PromisePayDotNet.Tests
{

    public class DynamicTokenTest : AbstractTest
    {
        //[Fact]
        //[Ignore("it seems I have created a token already, so it return error")]
        public void RequestToken()
        {
            //var repo = new TokenRepository(new RestClient());
            //var token = repo.RequestToken();
        }

        //[Fact]
        //[Ignore("Not implemented yet")]
        public void RequestSessionToken()
        {
            //var repo = new TokenRepository(new RestClient());
            //var result = repo.RequestSessionToken(new Dictionary<string,object>
            //{
            //    {"current_user_id" , "ec9bf096-c505-4bef-87f6-18822b9dbf2c"},
            //    {"amount" , "$100.00"},
            //    {"external_item_id" , "b3b4d024-a6de-4b04-9f8d-6545eef3b28f"},
            //    {"external_buyer_id" , "fdf58725-96bd-4bf8-b5e6-9b61be20662e"},
            //    {"external_seller_id" , "ec9bf096-c505-4bef-87f6-18822b9dbf2c"},
            //    {"buyer_email" , "aaa@bbb.com"},
            //    {"seller_email" , "ccc@ddd.com"},
            //    {"buyer_country" , "AUS"},
            //    {"seller_country" , "AUS"},
            //    {"buyer_first_name" , "Preved"},
            //    {"seller_first_name" , "Medved"},
            //    {"item_name" , "Bear"}
            //});
        }

        //[Fact]
        //[Ignore("Not implemented yet")]
        public void Widget()
        {
            //var repo = new TokenRepository(new RestClient());
            //var widget = repo.GetWidget("aaa-bbb-cc");
            throw new System.Exception();
        }

        [Fact]
        public void GenerateCardToken()
        {
            var content = Files.ReadAllText("./Fixtures/generate_card_token.json");

            var client = GetMockClient(content);
            var repo = Get<ITokenRepository>(client.Object);
            var result = repo.GenerateCardToken("card", "064d6800-fff3-11e5-86aa-5e5517507c66");
            Assert.Equal("card", result.TokenType);
            Assert.Equal("6e37598a3b33582b1dfcf13d5e2e45e3", result.Token);
            Assert.Equal("064d6800-fff3-11e5-86aa-5e5517507c66", result.UserId);
        }
    }
}
