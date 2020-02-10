namespace Catalogue.API.DataModels
{
	public class CatalogueItem
	{
		public CatalogueItem() { }

		public int Id { get; set; }
		public string Name { get; set; }

		public string Description { get; set; }
		public decimal Price { get; set; }
		public int AvailableStock { get; set; }

		//Numerous other properties that would be required for a catalogue item, 
		//	such as description, picture, review, category, supplier ID
	}
}
