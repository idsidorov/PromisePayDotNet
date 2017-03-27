using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PromisePayDotNet.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class UploadRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                    PromisePayDotNet.Dynamic.Interfaces.IUploadRepository
    {
        public UploadRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<UploadRepository>(), options)
        {
        }


        public IList<IDictionary<string,object>> ListUploads()
        {
            var request = new RestRequest("/uploads", Method.GET);

            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("uploads"))
            {
                var uploadCollection = dict["uploads"];
                return JsonConvert.DeserializeObject<List<IDictionary<string,object>>>(JsonConvert.SerializeObject(uploadCollection));
            }
            return new List<IDictionary<string,object>>();
        }

        public IDictionary<string, object> GetUploadById(string uploadId)
        {
            AssertIdNotNull(uploadId);
            var request = new RestRequest("/uploads/{id}", Method.GET);
            request.AddUrlSegment("id", uploadId);
            var response = SendRequest(Client, request);
            var result = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content).Values.First();
            return JsonConvert.DeserializeObject<IDictionary<string,object>>(JsonConvert.SerializeObject(result));
        }

        public IDictionary<string, object> CreateUpload(string csvData)
        {
            if (String.IsNullOrEmpty(csvData))
            {
                throw new ArgumentException("csvData cannot be empty");
            }

            var request = new RestRequest("/uploads", Method.POST);
            request.AddParameter("import", csvData);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("uploads"))
            {
                var itemCollection = dict["uploads"];
                return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

        public IDictionary<string, object> GetStatus(string uploadId)
        {
            AssertIdNotNull(uploadId);
            var request = new RestRequest("/uploads/{id}/import", Method.GET);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("uploads"))
            {
                var itemCollection = dict["uploads"];
                return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

        public IDictionary<string, object> StartImport(string uploadId)
        {
            AssertIdNotNull(uploadId);
            var request = new RestRequest("/uploads/{id}/import", Method.PATCH);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("uploads"))
            {
                var itemCollection = dict["uploads"];
                return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

    }
}
