using Hazelcast;

namespace Cache.Services
{
    public interface IOrderMapService
    {
        Task<string> GetOrderByKeyAsync(string key, CancellationToken token = default);
    }

    internal sealed class OrderMapService : IOrderMapService
    {
        private readonly HazelcastOptions _options;
        private readonly string _mapName;

        public OrderMapService(HazelcastOptions options, string mapName)
        {
            _options = options;
            _mapName = mapName;
        }

        public async Task<string> GetOrderByKeyAsync(string key, CancellationToken token = default)
        {
            try
            {
                await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token).ConfigureAwait(false);
                await using var map = await client.GetMapAsync<string, string>(_mapName).ConfigureAwait(false);
                var order = await map.FirstOrDefaultAsync(cancellationToken: token).ConfigureAwait(false);
                await client.DestroyAsync(map).ConfigureAwait(false);

                return order.Value;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
