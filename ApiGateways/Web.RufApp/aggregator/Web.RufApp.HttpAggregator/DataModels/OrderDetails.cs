namespace Web.RufApp.HttpAggregator.DataModels
{
	// The combination of order and catalogue details that we want to see at once. This is a lame example, combining the product price with the
	// order details. A better example might be obtaining a full description and an image
	public class OrderDetails
	{
		public OrderDetails() { }

		public int Id { get; set; }

		public string CustomerName { get; set; }

		public int ProductId { get; set; }

		public int QuantityOrdered { get; set; }

		public string ProductName { get; set; }

		public string ProductDescription { get; set; }
	}
}
