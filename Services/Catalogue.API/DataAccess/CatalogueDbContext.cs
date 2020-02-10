using Catalogue.API.DataAccess.Configurations;
using Catalogue.API.DataModels;
using Microsoft.EntityFrameworkCore;


namespace Catalogue.API.DataAccess
{
	public class CatalogueDbContext : DbContext
	{
		public CatalogueDbContext(DbContextOptions<CatalogueDbContext> options)
			: base(options)
		{ }

		//Any reference to this item list is a direct reference to the database table contents, thanks to the magic of EF
		public DbSet<CatalogueItem> CatalogueItems { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.HasSequence<int>(@"catalogue_hilo")
				.IncrementsBy(20);

			builder.ApplyConfiguration(new CatalogueItemConfiguration());
		}
	}
}
