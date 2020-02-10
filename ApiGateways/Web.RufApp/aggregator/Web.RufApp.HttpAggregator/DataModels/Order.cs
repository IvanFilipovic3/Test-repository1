namespace Web.RufApp.HttpAggregator.DataModels
{
	// The basis of Micro-services is to keep them de-coupled. As such, we do not reference the full Order item in the Orders
	// service. Rather we create a version that holds the information we're interested in
	public class Order
	{
		public Order() { }

		public int Id { get; set; }
		public string CustomerName { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
	}
}
