using Hazelcast;

namespace Cache.Services
{
    public interface IHazelcastService<TKey, TValue>
    {
        Task DeleteRecordAsync(TKey key);
        Task<TValue> GetRecordAsync(TKey key);
        Task PutRecordAsync(TKey key, TValue value);
        Task SetRecordAsync(TKey key, TValue value);
    }

    public sealed class HazelcastService<TKey, TValue> : IHazelcastService<TKey, TValue>
    {
        private readonly IHazelcastClient _client;
        private readonly string _map;

        public HazelcastService(HazelcastOptions options, string map)
        {
            _client = HazelcastClientFactory.StartNewClientAsync(options).GetAwaiter().GetResult();
            _map = map;
        }

        public async Task<TValue> GetRecordAsync(TKey key)
        {
            var worker = new HazelcastWorker<TKey, TValue>(_client, _map);
            var rec = await worker.GetRecordAsync(key).ConfigureAwait(false);
            return rec;
        }

        public async Task SetRecordAsync(TKey key, TValue value)
        {
            var worker = new HazelcastWorker<TKey, TValue>(_client, _map);
            await worker.SetRecordAsync(key, value).ConfigureAwait(false);
        }

        public async Task PutRecordAsync(TKey key, TValue value)
        {
            var worker = new HazelcastWorker<TKey, TValue>(_client, _map);
            await worker.PutRecordAsync(key, value).ConfigureAwait(false);
        }

        public async Task DeleteRecordAsync(TKey key)
        {
            var worker = new HazelcastWorker<TKey, TValue>(_client, _map);
            await worker.DeleteRecordAsync(key).ConfigureAwait(false);
        }
    }
}
