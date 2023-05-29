using Hazelcast;
using Model;

namespace Cache.Services
{
    public interface ICitiesService
    {
        Task<IEnumerable<City>> LoadCitiesAsync(CancellationToken token = default);
        Task<IEnumerable<City>> LoadCitiesByCountryNameAsync(string countryName, CancellationToken token = default);
        Task<IEnumerable<CityWithPopulation>> LoadCitiesWithPopulationAsync(CancellationToken token = default);
        Task<IEnumerable<CityWithPopulationArea>> LoadCitiesWithPopulationAreaAsync(CancellationToken token = default);
        Task<IEnumerable<CityWithPopulationAreaMayor>> LoadCitiesWithPopulationAreaMayorAsync(CancellationToken token = default);
        Task<IEnumerable<CityWithPopulationAreaMayorCountry>> LoadCitiesWithPopulationAreaMayorCountryAsync(CancellationToken token = default);
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
            try
            {
                await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token);

                await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT c.country, c.Name AS city FROM city AS c
ORDER BY c.Name;
", cancellationToken: token);

                return await result.Select(row =>
                        new City
                        {
                            CityName = row.GetColumn<string>("city"),
                            CountryName = row.GetColumn<string>("country"),
                        }
                    ).ToListAsync(cancellationToken: token);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<City>> LoadCitiesByCountryNameAsync(string countryName, CancellationToken token = default)
        {
            try
            {
                await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token);

                await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT c.country, c.Name AS city FROM city AS c
WHERE c.country LIKE '%{countryName?.Trim()}%'
ORDER BY c.Name;
", cancellationToken: token);

                return await result.Select(row =>
                        new City
                        {
                            CityName = row.GetColumn<string>("city"),
                            CountryName = row.GetColumn<string>("country"),
                        }
                    ).ToListAsync(cancellationToken: token);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CityWithPopulation>> LoadCitiesWithPopulationAsync(CancellationToken token = default)
        {
            try
            {
                await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token);

                await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT city.country, city.Name AS city, population2020.population AS population
FROM city
JOIN population2020
ON city.Name = population2020.city
ORDER BY population2020.population DESC;
", cancellationToken: token);

                return await result.Select(row =>
                        new CityWithPopulation
                        {
                            CityName = row.GetColumn<string>("city"),
                            CountryName = row.GetColumn<string>("country"),
                            Population = row.GetColumn<int>("population"),
                        }
                    ).ToListAsync(cancellationToken: token);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CityWithPopulationArea>> LoadCitiesWithPopulationAreaAsync(CancellationToken token = default)
        {
            try
            {
                await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token);

                await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT 
city.country
,city.Name AS city
,population2020.population AS population
,area.area
FROM city
JOIN population2020
ON city.Name = population2020.city
JOIN area
ON city.Name = area.city
ORDER BY area.area DESC;
", cancellationToken: token);

                return await result.Select(row =>
                        new CityWithPopulationArea
                        {
                            CityName = row.GetColumn<string>("city"),
                            CountryName = row.GetColumn<string>("country"),
                            Population = row.GetColumn<int>("population"),
                            Area = row.GetColumn<double>("area"),
                        }
                    ).ToListAsync(cancellationToken: token);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CityWithPopulationAreaMayor>> LoadCitiesWithPopulationAreaMayorAsync(CancellationToken token = default)
        {
            try
            {
                await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token);

                await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT 
city.country
,city.Name AS city
,population2020.population AS population
,area.area
,mayor.Name AS mayor
,mayor.electedYear
FROM mayor
JOIN city ON mayor.city = city.Name
JOIN population2020
ON city.Name = population2020.city
JOIN area
ON city.Name = area.city
ORDER BY mayor.electedYear DESC;
", cancellationToken: token);

                return await result.Select(row =>
                        new CityWithPopulationAreaMayor
                        {
                            CityName = row.GetColumn<string>("city"),
                            CountryName = row.GetColumn<string>("country"),
                            Population = row.GetColumn<int>("population"),
                            Area = row.GetColumn<double>("area"),
                            MayorName = row.GetColumn<string>("mayor"),
                            ElectedYear = row.GetColumn<int>("electedYear"),
                        }
                    ).ToListAsync(cancellationToken: token);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CityWithPopulationAreaMayorCountry>> LoadCitiesWithPopulationAreaMayorCountryAsync(CancellationToken token = default)
        {
            try
            {
                await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token);

                await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT 
ct.Name AS city
,p.population AS cityPopulation
,a.area AS cityArea
,m.Name AS mayor
,m.electedYear
,c.Name AS country
,c.dialingCode
,c.primeMinister
,c.currency
,c.population AS countryPopulation
,c.officialLanguage
FROM city AS ct
JOIN country AS c ON c.Name = ct.country
JOIN population2020 AS p ON p.city = ct.Name
JOIN area AS a ON a.city = ct.Name
JOIN mayor AS m ON m.city = ct.Name
ORDER BY ct.__key ASC;
", cancellationToken: token);

                return await result.Select(row =>
                        new CityWithPopulationAreaMayorCountry
                        {
                            CityName = row.GetColumn<string>("city"),
                            CityPopulation = row.GetColumn<int>("cityPopulation"),
                            CityArea = row.GetColumn<double>("cityArea"),
                            MayorName = row.GetColumn<string>("mayor"),
                            ElectedYear = row.GetColumn<int>("electedYear"),
                            CountryName = row.GetColumn<string>("country"),
                            DialingCode = row.GetColumn<string>("dialingCode"),
                            PrimeMinister = row.GetColumn<string>("primeMinister"),
                            Currency = row.GetColumn<string>("currency"),
                            CountryPopulation = row.GetColumn<double>("countryPopulation"),
                            OfficialLanguage = row.GetColumn<string>("officialLanguage"),
                        }
                    ).ToListAsync(cancellationToken: token);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
