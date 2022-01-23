using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using PropertyManagement.Helpers;
using PropertyManagement.Models;
using PropertyManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PropertyManagement.Implementations
{

    public class PropertyService : IPropertyService
    {
        private readonly ILogger<PropertyService> _logger;
        private readonly IElasticClient _client;

        public PropertyService(ILogger<PropertyService> logger, IElasticClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<ApiResponse> SearchProperty(SearchPropertyRequest request)
        {
            try
            {
                var propertyReponse = new List<Property>();

                var response = await _client.SearchAsync<Property>(x =>
                    x.Query(q =>
                        q.MultiMatch(m =>
                            m.Fields(f => f
                                .Field(n => n.name)
                                .Field(f => f.formerName)
                                .Field(s => s.streetAddress)
                                .Field(s => s.state)
                                .Field(m => m.market)
                                .Field(c => c.city)
                              )
                            .Query(request.Query.Trim())
                          )
                       )
                    );

                request.Limit = request.Limit < 1 ? 25 : request.Limit;

                if (request.Page < 1) //Page must not be less than 1
                    request.Page = 1;

                int skip = (request.Page - 1) * request.Limit;
                var tt = response.Documents.ToList();
                propertyReponse = response.Documents
                    .OrderByDescending(x => x.propertyID)
                    .Skip(skip)
                    .Take(request.Limit)
                    .ToList();

                if (propertyReponse.Count == 0)
                    return new ApiResponse((int)HttpStatusCode.NotFound, "Not found", null);

                return new ApiResponse((int)HttpStatusCode.OK, "Successful", propertyReponse);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occured: " + e.Message);
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "Error occured, please try again.", null);
            }
        }
    }
}