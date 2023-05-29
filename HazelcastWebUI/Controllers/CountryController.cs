using Cache.Services;
using Microsoft.AspNetCore.Mvc;

namespace HazelcastWebUI.Controllers
{
    public sealed class CountryController : Controller
    {
        private readonly ICountriesService _countriesService;

        public CountryController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        public async Task<IActionResult> Countries()
        {
            var countries = await _countriesService.LoadCountriesAsync().ConfigureAwait(false);
            return View(countries);
        }
    }
}
