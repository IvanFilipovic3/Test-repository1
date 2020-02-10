using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.API.DataModels;

namespace Orders.API.DataAccess.Configurations
{
	internal class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.ToTable(@"Order");

			builder.HasKey(ci => ci.Id);

			//use a sequence, rather than IDENTITY, to reduce round trip to the database when obtaining a new unique ID
			builder.Property(ci => ci.Id)
				.ForSqlServerUseSequenceHiLo(@"order_hilo")
				.ValueGeneratedOnAdd()
				.IsRequired();

			builder.Property(ci => ci.CustomerName)
				.IsRequired(true)				
				.HasMaxLength(50);

			builder.Property(ci => ci.ProductId)
				.IsRequired(true);

			builder.Property(ci => ci.ProductName)
				.IsRequired(true)
				.HasMaxLength(50);

			builder.Property(ci => ci.Quantity)
				.IsRequired(true);
		}
	}
}
