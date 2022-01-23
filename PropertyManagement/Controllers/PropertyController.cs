using Microsoft.AspNetCore.Mvc;
using PropertyManagement.Models;
using PropertyManagement.Services;
using System.Threading.Tasks;

namespace PropertyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchProperty(SearchPropertyRequest request)
        {
            var result = await _propertyService.SearchProperty(request);

            if (result.Code == 200)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
