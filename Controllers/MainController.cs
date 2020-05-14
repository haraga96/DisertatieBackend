using System.Threading.Tasks;
using Backend_Dis_App.BaseClasses;
using Backend_Dis_App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Dis_App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/")]
    public class MainController : BaseController
    {
        private readonly ICountryService _countryService;
        public MainController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        
        [HttpGet]
        [Route("mainpage")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return Ok(countries);
        }
    }
}