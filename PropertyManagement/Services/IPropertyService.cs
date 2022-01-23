using PropertyManagement.Helpers;
using PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagement.Services
{
    public interface IPropertyService
    {
        /// <summary>
        /// Search for Properties using the search criteria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse> SearchProperty(SearchPropertyRequest request);
    }
}