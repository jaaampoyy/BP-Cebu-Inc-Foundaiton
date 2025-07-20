using Microsoft.Data.SqlClient;

namespace Classroom.Utilities
{
    public static class SqlDataReaderExtensions
    {
        public static DateTime? GetNullableDateTime(this SqlDataReader reader, string name)
        {
            var col = reader.GetOrdinal(name);
            return reader.IsDBNull(col) ? null : reader.GetDateTime(col);
        }
    }
}
