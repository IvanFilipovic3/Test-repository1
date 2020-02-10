using Microsoft.EntityFrameworkCore;

using Orders.API.DataAccess.Configurations;
using Orders.API.DataModels;

namespace Orders.API.DataAccess
{
	public class OrderDbContext : DbContext
	{
		public OrderDbContext(DbContextOptions<OrderDbContext> options)
			: base(options)
		{ }

		//Any reference to this item list is a direct reference to the database table contents, thanks to the magic of EF
		public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.HasSequence<int>(@"order_hilo")
				.IncrementsBy(20);

			builder.ApplyConfiguration(new OrderConfiguration());
		}
	}
}
