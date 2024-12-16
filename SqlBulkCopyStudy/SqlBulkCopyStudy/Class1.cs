using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace SqlBulkCopyStudy
{
    public class Class1
    {
        public void Foo()
        {
            //using IDataReader reader = null!;
            //using SqlConnection connection = null!;
            //using SqlBulkCopy sqlBulkCopy = new(connection);

            //sqlBulkCopy.ColumnMappings.Add("source", "destination");
            //sqlBulkCopy.ColumnMappings.Add(1, "destination");
            //sqlBulkCopy.ColumnMappings.Add("source", 2);
            //sqlBulkCopy.ColumnMappings.Add(1, 2);

            using Stream stream = null!;
            using FixedLengthFileReader reader = new(stream, Encoding.UTF8, "\r\n");

            while (reader.Read())
            {
                var field1 = reader.GetField(0, 10);
                var field2 = reader.GetField(20, 3);
            }

        }
    }
}
