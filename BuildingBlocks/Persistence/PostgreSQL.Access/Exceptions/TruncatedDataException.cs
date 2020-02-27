using System;

namespace Prophet.SaaS.Database.Access.Exceptions
{
	/// <summary>
	/// Provides specific Exception for data that would be truncated on update or insert.
	/// </summary>
	public class TruncatedDataException : Exception
	{
		/// <inheritdoc/>
		public TruncatedDataException()
			: base() { }

		/// <inheritdoc/>
		public TruncatedDataException(string message, Exception inner)
			: base(message, inner) { }

		/// <inheritdoc/>
		public TruncatedDataException(string message)
			: base(message) { }

		/// <summary>
		/// Create exception with a custom error message based on the expected size of the data column and the
		/// actual size of the data used to update that column.
		/// </summary>
		/// <param name="dataName">Name of the database column that would have overflowed.</param>
		/// <param name="dataSize">The size of the date attempting to be inserted into the data column.</param>
		/// <param name="maxSize">The maximum size the data column can hold.</param>
		public TruncatedDataException(string dataName, int dataSize, int maxSize)
			: base($"Data size too long for column {dataName}. Maximum size is {maxSize} but data size is {dataSize}")
		{
		}
	}
}
