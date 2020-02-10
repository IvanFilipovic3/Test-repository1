namespace Web.RufApp.HttpAggregator.DataModels
{
	// The basis of Micro-services is to keep them de-coupled. As such, we do not reference the full catalogue item in the catalogue
	// service. Rather we create a version that holds the information we're interested in
	public class CatalogueItem
	{
		public CatalogueItem() { }

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}
}
