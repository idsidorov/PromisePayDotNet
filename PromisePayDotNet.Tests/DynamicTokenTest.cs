using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using RestSharp;
using System.Collections.Generic;
using System.IO;

namespace PromisePayDotNet.Tests
{

    public class DynamicTokenTest : AbstractTest
    {
        [Test]
        [Ignore("it seems I have created a token already, so it return error")]
        public void RequestToken()
        {
            var repo = new TokenRepository(new RestClient(), GetMockSettings().Object, GetMockLogger<TokenRepository>().Object);
            var token = repo.RequestToken();
        }

        [Test]
        [Ignore("Not implemented yet")]
        public void RequestSessionToken()
        {
            var repo = new TokenRepository(new RestClient(), GetMockSettings().Object, GetMockLogger<TokenRepository>().Object);
            var result = repo.RequestSessionToken(new Dictionary<string,object>
            {
                {"current_user_id" , "ec9bf096-c505-4bef-87f6-18822b9dbf2c"},
                {"amount" , "$100.00"},
                {"external_item_id" , "b3b4d024-a6de-4b04-9f8d-6545eef3b28f"},
                {"external_buyer_id" , "fdf58725-96bd-4bf8-b5e6-9b61be20662e"},
                {"external_seller_id" , "ec9bf096-c505-4bef-87f6-18822b9dbf2c"},
                {"buyer_email" , "aaa@bbb.com"},
                {"seller_email" , "ccc@ddd.com"},
                {"buyer_country" , "AUS"},
                {"seller_country" , "AUS"},
                {"buyer_first_name" , "Preved"},
                {"seller_first_name" , "Medved"},
                {"item_name" , "Bear"}
            });
        }

        [Test]
        [Ignore("Not implemented yet")]
        public void Widget()
        {
            var repo = new TokenRepository(new RestClient(), GetMockSettings().Object, GetMockLogger<TokenRepository>().Object);
            var widget = repo.GetWidget("aaa-bbb-cc");
        }

        [Test]
        public void GenerateCardToken()
        {
            var content = File.ReadAllText("./Fixtures/generate_card_token.json");

            var client = GetMockClient(content);
            var repo = new TokenRepository(client.Object, GetMockSettings().Object, GetMockLogger<TokenRepository>().Object);
            var resp = repo.GenerateCardToken("card", "064d6800-fff3-11e5-86aa-5e5517507c66");
            var token = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp["token_auth"]));
            Assert.AreEqual("card", token["token_type"]);
            Assert.AreEqual("6e37598a3b33582b1dfcf13d5e2e45e3", token["token"]);
            Assert.AreEqual("064d6800-fff3-11e5-86aa-5e5517507c66", token["user_id"]);
        }
    }
}
