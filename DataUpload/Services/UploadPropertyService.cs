using DataUpload.Models;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataUpload.Services
{
    public class UploadPropertyService
    {
        private readonly IConfiguration _configuration;
        private readonly string _elasticSearchUrl;
        private readonly string _userName;
        private readonly string _password;

        public UploadPropertyService(IConfiguration configuration)
        {
            _configuration = configuration;
            _elasticSearchUrl = _configuration["ElasticSearchUrl"];
            _userName = _configuration["UserName"];
            _password = _configuration["Password"];
        }

        /// <summary>
        /// Fetch the properties data from the json file to upload to ElasticSearch
        /// </summary>
        /// <returns></returns>
        public async Task<List<PropertyModel>> PrepareDataToUpload()
        {
            try
            {
                StreamReader streamReader = new StreamReader("properties.json");
                string propertyString = await streamReader.ReadToEndAsync();
                List<PropertyModel> propertyModel = JsonConvert.DeserializeObject<List<PropertyModel>>(propertyString);
                return propertyModel;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured while reading Property json data: " + e.Message);
                throw;
            }
        }

        /// <summary>
        /// Upload the properties data 
        /// </summary>
        /// <returns></returns>
        public async Task UploadProperties()
        {
            try
            {
                var propertyData = await PrepareDataToUpload();

                //Create the connection for the upload
                var uri = new Uri(_elasticSearchUrl);
                var settings = new ConnectionSettings(uri)
                    .BasicAuthentication(_userName, _password);
                var client = new ElasticClient(settings);

                var propertiesToUpload = propertyData.Select(x => x.property).ToList();

                //Bulk upload the properties
                var bulkAllObservable = client.BulkAll(propertiesToUpload, p => p.Index("property")
                    .BackOffTime("30s")
                    .BackOffRetries(2)
                    .RefreshOnCompleted()
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    .Size(1000))//upload 1000 documents at each iteration
                        .Wait(TimeSpan.FromMinutes(1), x =>
                        {
                            Console.WriteLine(x.Page);
                        });
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured: " + e.Message);
            }
        }
    }
}