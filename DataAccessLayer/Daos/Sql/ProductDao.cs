using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Daos.Sql;

internal class ProductDao(IDataContext<SqlConnection> dataContext) : BaseDao<SqlConnection>(dataContext), IDao<Product>
{
    public Product Create(Product entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Product entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> Read()
    {
        string sql = "SELECT * FROM Products";

        using IDbConnection connection = DataContext.Open();

        IDbCommand command = connection.CreateCommand();
        command.CommandText = sql;

        IDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            yield return new Product()
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Price = reader.GetDecimal(3)
            };
        }
    }

    public bool Update(Product entity)
    {
        throw new NotImplementedException();
    }
}
