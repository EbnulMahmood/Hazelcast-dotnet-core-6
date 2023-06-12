using Hazelcast;
using Model;

namespace Cache.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Customer>> LoadCustomerAsync(CancellationToken token = default);
        Task<IEnumerable<CustomerOrder>> LoadOrderAsync(CancellationToken token = default);
        Task<IEnumerable<OrderWithCustomer>> LoadOrderWithCustomerAsync(CancellationToken token = default);
        Task SeedDataAsync(int start, int totalRecord, CancellationToken token = default);
    }

    public sealed class OrderService : IOrderService
    {
        private readonly HazelcastOptions _options;

        public OrderService(HazelcastOptions options)
        {
            _options = options;
        }

        public async Task<IEnumerable<Customer>> LoadCustomerAsync(CancellationToken token = default)
        {
            try
            {
                await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token).ConfigureAwait(false);

                await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT
__key AS id
,name
,address
FROM customer;
", cancellationToken: token).ConfigureAwait(false);

                //await client.DisposeAsync().ConfigureAwait(false);

                return await result.Select(row =>
                        new Customer
                        {
                            id = row.GetColumn<int>("id"),
                            name = row.GetColumn<string>("name"),
                            address = row.GetColumn<string>("address"),
                        }
                    ).ToListAsync(cancellationToken: token).ConfigureAwait(false);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CustomerOrder>> LoadOrderAsync(CancellationToken token = default)
        {
            try
            {
                await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token).ConfigureAwait(false);

                await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT
__key AS id
,customerId
,items
,price
FROM customerOrder;
", cancellationToken: token).ConfigureAwait(false);

                return await result.Select(row =>
                        new CustomerOrder
                        {
                            id = row.GetColumn<int>("id"),
                            customerId = row.GetColumn<int>("customerId"),
                            items = row.GetColumn<string>("items"),
                            price = row.GetColumn<double>("price"),
                        }
                    ).ToListAsync(cancellationToken: token).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderWithCustomer>> LoadOrderWithCustomerAsync(CancellationToken token = default)
        {
            try
            {
                await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token).ConfigureAwait(false);

                await using var result = await client.Sql.ExecuteQueryAsync(@$"
SELECT
co.__key AS id
,co.items AS items
,co.price AS price
,c.name AS customerName
,c.address AS customerAddress
FROM customerOrder AS co
INNER JOIN customer AS c
ON co.customerId = c.__key;
", cancellationToken: token).ConfigureAwait(false);

                return await result.Select(row =>
                        new OrderWithCustomer
                        {
                            Id = row.GetColumn<int>("id"),
                            Items = row.GetColumn<string>("items"),
                            Price = row.GetColumn<double>("price"),
                            CustomerName = row.GetColumn<string>("customerName"),
                            CustomerAddress = row.GetColumn<string>("customerAddress"),
                        }
                    ).ToListAsync(cancellationToken: token).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SeedDataAsync(int start, int totalRecord, CancellationToken token = default)
        {
            await using var client = await HazelcastClientFactory.StartNewClientAsync(_options, cancellationToken: token).ConfigureAwait(false);

            await using var customers = await client.GetMapAsync<int, Customer>("customer").ConfigureAwait(false);
            //await customers.DestroyAsync().ConfigureAwait(false);
            await using var orders = await client.GetMapAsync<int, CustomerOrder>("customerOrder").ConfigureAwait(false);
            //await orders.DestroyAsync().ConfigureAwait(false);

            for (int i = start; i < totalRecord; i++)
            {
                var newCustomer = new Customer { id = i + 1, name = $"Name_{i + 1}", address = $"Address_{i + 1}" };
                await customers.SetAsync(i + 1, newCustomer).ConfigureAwait(false);

                var newOrder = new CustomerOrder { customerId = newCustomer.id, items = $"Items_{i + 1}", price = i + 1 };
                await orders.SetAsync(i + 1, newOrder).ConfigureAwait(false);
            }
        }
    }
}
