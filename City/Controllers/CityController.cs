using City.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace City.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IConsulHttpClient _consulClient;

        public CityController(ICityService cityService, IConsulHttpClient consulClient)
        {
            _cityService = cityService;
            _consulClient = consulClient;
        }

        [HttpPost]
        public async Task<List<City.Models.City>> AddCity(City.Models.City city)
        {
            return await _cityService.AddCity(city);
        }

        [HttpGet]
        public async Task<List<City.Models.City>> GetAllCity()
        {
            var a = await _consulClient.GetAsync<dynamic>("StateService", $"/State/GetAllState");
            return await _cityService.GetAllCity();
        }
    }
}
