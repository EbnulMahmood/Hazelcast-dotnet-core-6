﻿using Cache.Services;
using Microsoft.AspNetCore.Mvc;

namespace HazelcastAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CitiesController : ControllerBase
    {
        private readonly ICitiesService _citiesService;

        public CitiesController(ICitiesService citiesService)
        {
            _citiesService = citiesService;
        }

        [HttpGet]
        [Route("/cities/load")]
        public async Task<JsonResult> LoadCities(CancellationToken token = default)
        {
            var cities = await _citiesService.LoadCitiesAsync(token).ConfigureAwait(false);

            return new JsonResult(cities);
        }

        [HttpGet]
        [Route("/cities/load-with-population")]
        public async Task<JsonResult> LoadCitiesWithPopulation(CancellationToken token = default)
        {
            var cities = await _citiesService.LoadCitiesWithPopulationAsync(token).ConfigureAwait(false);

            return new JsonResult(cities);
        }

        [HttpGet]
        [Route("/cities/load-with-population-area")]
        public async Task<JsonResult> LoadCitiesWithPopulationArea(CancellationToken token = default)
        {
            var cities = await _citiesService.LoadCitiesWithPopulationAreaAsync(token).ConfigureAwait(false);

            return new JsonResult(cities);
        }

        [HttpGet]
        [Route("/cities/load-with-population-area-mayor")]
        public async Task<JsonResult> LoadCitiesWithPopulationAreaMayor(CancellationToken token = default)
        {
            var cities = await _citiesService.LoadCitiesWithPopulationAreaMayorAsync(token).ConfigureAwait(false);

            return new JsonResult(cities);
        }

        [HttpGet]
        [Route("/cities/load-by-country/{country}")]
        public async Task<JsonResult> LoadCities(string country, CancellationToken token = default)
        {
            var cities = await _citiesService.LoadCitiesByCountryNameAsync(country, token).ConfigureAwait(false);

            return new JsonResult(cities);
        }

    }
}
