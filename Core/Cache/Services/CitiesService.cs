﻿using Hazelcast;
using Model;

namespace Cache.Services
{
    public interface ICitiesService
    {
        Task<IEnumerable<City>> LoadCitiesAsync(CancellationToken token = default);
        Task<IEnumerable<City>> LoadCitiesByCountryNameAsync(string countryName, CancellationToken token = default);
        Task<IEnumerable<CityWithPopulation>> LoadCitiesWithPopulationAsync(CancellationToken token = default);
        Task<IEnumerable<CityWithPopulationArea>> LoadCitiesWithPopulationAreaAsync(CancellationToken token = default);
    }

    public sealed class CitiesService : ICitiesService
    {
        private readonly HazelcastOptions _options;

        public CitiesService(HazelcastOptions options)
        {
            _options = options;
        }

        public async Task<IEnumerable<City>> LoadCitiesAsync(CancellationToken token = default)
        {
            await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token);

            await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT c.country, c.city FROM city AS c
ORDER BY c.city;
", cancellationToken: token);

            var cityList = new List<City>();
            await foreach (var row in result)
            {
                cityList.Add(new City
                {
                    CityName = row.GetColumn<string>("city"),
                    CountryName = row.GetColumn<string>("country"),
                });
            }

            return cityList;
        }

        public async Task<IEnumerable<City>> LoadCitiesByCountryNameAsync(string countryName, CancellationToken token = default)
        {
            await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token);

            await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT c.country, c.city FROM city AS c
WHERE c.country LIKE '%{countryName?.Trim()}%'
ORDER BY c.city;
", cancellationToken: token);

            var cityList = new List<City>();
            await foreach (var row in result)
            {
                cityList.Add(new City
                {
                    CityName = row.GetColumn<string>("city"),
                    CountryName = row.GetColumn<string>("country"),
                });
            }

            return cityList;
        }

        public async Task<IEnumerable<CityWithPopulation>> LoadCitiesWithPopulationAsync(CancellationToken token = default)
        {
            await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token);

            await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT city.country, city.city, population2020.population AS population
FROM city
JOIN population2020
ON city.city = population2020.city;
", cancellationToken: token);

            var cityList = new List<CityWithPopulation>();
            await foreach (var row in result)
            {
                cityList.Add(new CityWithPopulation
                {
                    CityName = row.GetColumn<string>("city"),
                    CountryName = row.GetColumn<string>("country"),
                    Population = row.GetColumn<int>("population"),
                });
            }

            return cityList;
        }

        public async Task<IEnumerable<CityWithPopulationArea>> LoadCitiesWithPopulationAreaAsync(CancellationToken token = default)
        {
            await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token);

            await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT 
city.country
,city.city
,population2020.population AS population
,area.area
FROM city
JOIN population2020
ON city.city = population2020.city
JOIN area
ON city.city = area.city
ORDER BY area.area DESC;
", cancellationToken: token);

            var cityList = new List<CityWithPopulationArea>();
            await foreach (var row in result)
            {
                cityList.Add(new CityWithPopulationArea
                {
                    CityName = row.GetColumn<string>("city"),
                    CountryName = row.GetColumn<string>("country"),
                    Population = row.GetColumn<int>("population"),
                    Area = row.GetColumn<double>("area"),
                });
            }

            return cityList;
        }
    }
}
