using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerTests
{
    internal class SqlServerDataContext : IDataContext<SqlConnection>
    {
        private readonly string _connectionString = "Server=192.168.56.101; Database=CafeSanchez; User Id=sa; Password=P@$$w0rd; TrustServerCertificate=True";

        public SqlConnection Open()
        {
            SqlConnection conn = new(_connectionString);
            conn.Open();

            return conn;
        }
    }
}
