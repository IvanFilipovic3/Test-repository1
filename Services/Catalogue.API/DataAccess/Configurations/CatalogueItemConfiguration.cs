
using Catalogue.API.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogue.API.DataAccess.Configurations
{
	internal class CatalogueItemConfiguration : IEntityTypeConfiguration<CatalogueItem>
	{
		public void Configure(EntityTypeBuilder<CatalogueItem> builder)
		{
			builder.ToTable(@"Catalogue");

			builder.HasKey(ci => ci.Id);

			//use a sequence, rather than IDENTITY, to reduce round trip to the database when obtaining a new unique ID
			builder.Property(ci => ci.Id)
				.ForSqlServerUseSequenceHiLo(@"catalogue_hilo")
				.ValueGeneratedOnAdd()
				.IsRequired();

			builder.Property(ci => ci.Name)
				.IsRequired(true)
				.HasMaxLength(50);

			builder.Property(ci => ci.Name)
				.HasMaxLength(200);

			builder.Property(ci => ci.Price)
				.HasColumnType(@"decimal(18,2)")
				.IsRequired(true);
		}
	}
}
