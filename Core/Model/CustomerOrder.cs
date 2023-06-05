namespace Model
{
    public readonly record struct Customer(int id, string name, string address);
    public readonly record struct CustomerOrder(int id, int customerId, string items, double price);
    public readonly record struct OrderWithCustomer(int Id, string Items, double Price, string CustomerName, string CustomerAddress);
}
