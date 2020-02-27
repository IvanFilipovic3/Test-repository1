using Prophet.SaaS.Database.Access;

namespace Prophet.SaaS.PostgreSQL.Access.Extensions
{
	public static class PgSqlExtensions
	{
		public static AdoDbSourceOptions UsePostgreSQL(this AdoDbSourceOptions settings, string connectionString)
		{
			settings.DbExectorCreator = (l) => new PgSqlDbObject(l, connectionString);

			return settings;
		}
	}
}
