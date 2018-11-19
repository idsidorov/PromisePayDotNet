using Newtonsoft.Json;
using PromisePayDotNet.DTO;
using PromisePayDotNet.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Implementations
{
    public class UploadRepository : AbstractRepository, IUploadRepository
    {
        public UploadRepository(IRestClient client, ISettings settings, ILogger<UploadRepository> logger) : base(client, settings, logger)
        {
        }

        public IList<Upload> ListUploads()
        {
            var request = new RestRequest("/uploads", Method.GET);

            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("uploads"))
            {
                var uploadCollection = dict["uploads"];
                return JsonConvert.DeserializeObject<List<Upload>>(JsonConvert.SerializeObject(uploadCollection));
            }
            return new List<Upload>();
        }

        public Upload GetUploadById(string uploadId)
        {
            AssertIdNotNull(uploadId);
            var request = new RestRequest("/uploads/{id}", Method.GET);
            request.AddUrlSegment("id", uploadId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Upload>>(response.Content).Values.First();
        }

        public Upload CreateUpload(string csvData)
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
                return JsonConvert.DeserializeObject<Upload>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

        public Upload GetStatus(string uploadId)
        {
            AssertIdNotNull(uploadId);
            var request = new RestRequest("/uploads/{id}/import", Method.GET);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("uploads"))
            {
                var itemCollection = dict["uploads"];
                return JsonConvert.DeserializeObject<Upload>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

        public Upload StartImport(string uploadId)
        {
            AssertIdNotNull(uploadId);
            var request = new RestRequest("/uploads/{id}/import", Method.PATCH);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("uploads"))
            {
                var itemCollection = dict["uploads"];
                return JsonConvert.DeserializeObject<Upload>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;            
        }
    }
}
