using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataAccessLayer.Daos.Sql;

internal class ProductDao(IDataContext<SqlConnection> dataContext) : BaseDao<SqlConnection>(dataContext), IDao<Product>
{
    string _selectProductsSql = "SELECT * FROM Products";
    string _selectProductSql = "SELECT * FROM Products WHERE Id = @id";
    string _updateProductSql = "UPDATE Products SET Price = @price WHERE Id = @id";

    public Product Create(Product entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> Read()
    {
        using IDbConnection connection = DataContext.Open();

        IDbCommand command = connection.CreateCommand();
        command.CommandText = _selectProductsSql;

        IDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            byte[] version = new byte[8];
            reader.GetBytes(4, 0, version, 0, 8);

            Product product = new()
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Price = reader.GetDecimal(3),
                Version = version
            };

            yield return product;
        }
    }

    public bool Update(Product entity)
    {
        using SqlConnection connection = DataContext.Open();

        SqlCommand selectCommand = new(_selectProductSql, connection);
        selectCommand.Parameters.AddWithValue("id", entity.Id);

        SqlDataReader reader = selectCommand.ExecuteReader();

        if (reader.Read())
        {
            byte[] version = new byte[8];
            reader.GetBytes(4, 0, version, 0, 8);

            Product product = new()
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Price = reader.GetDecimal(3),
                Version = version
            };
            reader.Close();

            if (product.Version.SequenceEqual(entity.Version))
            {
                SqlCommand updateCommand = new(_updateProductSql, connection);
                updateCommand.Parameters.AddWithValue("id", entity.Id);
                updateCommand.Parameters.AddWithValue("price", entity.Price);

                int result = updateCommand.ExecuteNonQuery();

                return result == 1;
            }
        }
        return false;
    }



    public bool Delete(Product entity)
    {
        throw new NotImplementedException();
    }
}
