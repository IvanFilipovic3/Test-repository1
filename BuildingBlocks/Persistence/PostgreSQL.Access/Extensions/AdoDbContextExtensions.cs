using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Prophet.SaaS.Database.Access.Extensions
{
	public static class AdoDbContextExtensions
	{
		public static IServiceCollection AddAdoDbContext<TContext, TSource>(this IServiceCollection services,
			[AllowNull]  Action<IServiceProvider, AdoDbSourceOptions<TSource>> configure)
			where TContext : AdoDbContext
			where TSource : DbSource
		{
			services.Add(
				new ServiceDescriptor(typeof(AdoDbSourceOptions<TSource>), p => CreateDbContextOptions<TSource>(p, configure), ServiceLifetime.Scoped));

			services.AddScoped<TSource>();
			services.AddScoped<TContext>();

			return services;
		}

		private static AdoDbSourceOptions<TSource> CreateDbContextOptions<TSource>(IServiceProvider applicationServiceProvider,
			[AllowNull] Action<IServiceProvider, AdoDbSourceOptions<TSource>> optionsAction)
			where TSource : DbSource
		{
			var options = new AdoDbSourceOptions<TSource>();
			optionsAction?.Invoke(applicationServiceProvider, options);

			return options;
		}
	}
}
