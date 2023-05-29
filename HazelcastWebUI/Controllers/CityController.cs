using Cache.Services;
using Microsoft.AspNetCore.Mvc;

namespace HazelcastWebUI.Controllers
{
    public sealed class CityController : Controller
    {
        private readonly ICitiesService _citiesService;

        public CityController(ICitiesService citiesService)
        {
            _citiesService = citiesService;
        }

        public async Task<IActionResult> Cities(CancellationToken token = default)
        {
            try
            {
                var cityList = await _citiesService.LoadCitiesAsync(token);
                return View(cityList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Populations(CancellationToken token = default)
        {
            try
            {
                var cityList = await _citiesService.LoadCitiesWithPopulationAsync(token);
                return View(cityList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Areas(CancellationToken token = default)
        {
            try
            {
                var cityList = await _citiesService.LoadCitiesWithPopulationAreaAsync(token);
                return View(cityList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Mayors(CancellationToken token = default)
        {
            try
            {
                var cityList = await _citiesService.LoadCitiesWithPopulationAreaMayorAsync(token);
                return View(cityList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CitiesDetails(CancellationToken token = default)
        {
            try
            {
                var cityList = await _citiesService.LoadCitiesWithPopulationAreaMayorCountryAsync(token);
                return View(cityList);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
