namespace Orders.API.DataModels
{
	public class Order
	{
		public Order() { }

		public int Id { get; set; }
		public string CustomerName { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }

		//Numerous other properties that would be required for a catalogue item, 
		//	such as description, picture, review, category, supplier ID
	}
}
