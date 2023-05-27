using Hazelcast;

namespace Cache.Services
{
    public sealed class HazelcastWorker<TKey, TValue>
    {
        private readonly IHazelcastClient _client;
        private readonly string _map;

        public HazelcastWorker(IHazelcastClient client, string map)
        {
            _client = client;
            _map = map;
        }

        public async Task<TValue> GetRecordAsync(TKey key) 
        {
            var map = await _client.GetMapAsync<TKey, TValue>(_map).ConfigureAwait(false);
            var rec = await map.GetAsync(key).ConfigureAwait(false);
            return rec;
        }

        public async Task SetRecordAsync(TKey key, TValue value)
        {
            var map = await _client.GetMapAsync<TKey, TValue>(_map).ConfigureAwait(false);
            await map.SetAsync(key, value).ConfigureAwait(false);
        }

        public async Task PutRecordAsync(TKey key, TValue value)
        {
            var map = await _client.GetMapAsync<TKey, TValue>(_map).ConfigureAwait(false);
            await map.PutAsync(key, value).ConfigureAwait(false);
        }

        public async Task DeleteRecordAsync(TKey key)
        {
            var map = await _client.GetMapAsync<TKey, TValue>(_map).ConfigureAwait(false);
            await map.DeleteAsync(key).ConfigureAwait(false);
        }
    }
}
