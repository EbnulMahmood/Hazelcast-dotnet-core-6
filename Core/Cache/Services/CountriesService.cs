using Hazelcast;
using Model;

namespace Cache.Services
{
    public interface ICountriesService
    {
        Task<IEnumerable<Country>> LoadCountriesAsync(CancellationToken token = default);
    }

    public sealed class CountriesService : ICountriesService
    {
        private readonly HazelcastOptions _options;

        public CountriesService(HazelcastOptions options)
        {
            _options = options;
        }

        public async Task<IEnumerable<Country>> LoadCountriesAsync(CancellationToken token = default)
        {
            try
            {
                await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token);

                await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT 
Name AS country,
dialingCode,
primeMinister,
currency,
population,
officialLanguage
FROM country;
", cancellationToken: token);

                return await result.Select(row =>
                        new Country
                        {
                            CountryName = row.GetColumn<string>("country"),
                            DialingCode = row.GetColumn<string>("dialingCode"),
                            PrimeMinister = row.GetColumn<string>("primeMinister"),
                            Currency = row.GetColumn<string>("currency"),
                            Population = row.GetColumn<double>("population"),
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
