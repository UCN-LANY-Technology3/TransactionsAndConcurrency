using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Daos.Sql;

/// <summary>
/// DAO class that handles orders and orderlines
/// </summary>
/// <param name="dataContext">The data context where data is stored</param>
internal class OrderDao(IDataContext<SqlConnection> dataContext) : BaseDao<SqlConnection>(dataContext), IDao<Order>
{
    private readonly string _selectOrdersSql = "SELECT * FROM Orders";
    private readonly string _selectOrderlinesSql = "SELECT * FROM Orderlines WHERE OrderId = @orderId";
    private readonly string _selectProductsSql = "SELECT * FROM Products ";
    private readonly string _updateOrderSql = "UPDATE Orders SET Status = @status WHERE Id = @id";
    private readonly string _insertOrderlineSql = "INSERT INTO Orderlines (OrderId, ProductId, Quantity) VALUES (@orderId, @productId, @quantity);";
    private readonly string _insertOrderSql = "INSERT INTO Orders (CustomerName, Date, Discount) VALUES (@customerName, @date, @discount);SELECT SCOPE_IDENTITY();";
    private readonly string _deleteOrderlinesSql = "DELETE FROM Orderlines WHERE OrderId = @orderId";
    private readonly string _deleteOrderSql = "DELETE FROM Orders WHERE Id = @id";


    public Order Create(Order entity)
    {
        // Create order

        using SqlConnection connection = DataContext.Open();
        using SqlTransaction transaction = connection.BeginTransaction(); // Default isolationlevel -> ReadCommitted

        try
        {
            SqlCommand createOrderCommand = new(_insertOrderSql, connection, transaction);
            createOrderCommand.Parameters.AddWithValue("customerName", entity.CustomerName);
            createOrderCommand.Parameters.AddWithValue("date", entity.Date);
            createOrderCommand.Parameters.AddWithValue("discount", entity.Discount);

            object? orderId = createOrderCommand.ExecuteScalar();
            entity.Id = Convert.ToInt32(orderId);

            // Create orderlines

            foreach (var orderline in entity.Orderlines)
            {
                SqlCommand createOrderlineCommand = new(_insertOrderlineSql, connection, transaction);
                createOrderlineCommand.Parameters.AddWithValue("orderId", entity.Id);
                createOrderlineCommand.Parameters.AddWithValue("productId", orderline.Item.Id);
                createOrderlineCommand.Parameters.AddWithValue("quantity", orderline.Quantity);

                createOrderlineCommand.ExecuteNonQuery();
            }
        }
        catch (SqlException ex)
        {
            transaction.Rollback();
            throw new DaoException("Could not create order", ex);
        }

        transaction.Commit();

        return entity;
    }

    public IEnumerable<Order> Read()
    {

        List<Order> orders = [];
        List<Product> products = [];

        using SqlConnection connection = DataContext.Open();
        using SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            SqlCommand selectCommand = new(_selectOrdersSql, connection, transaction);
            SqlDataReader ordersReader = selectCommand.ExecuteReader();

            // get orders
            while (ordersReader.Read())
            {
                orders.Add(new Order()
                {
                    Id = Convert.ToInt32(ordersReader["Id"]),
                    CustomerName = Convert.ToString(ordersReader["CustomerName"]) ?? throw new ArgumentNullException("CustomerName is required"),
                    Discount = Convert.ToDecimal(ordersReader["Discount"]),
                    Status = Convert.ToString(ordersReader["Status"]) ?? throw new ArgumentNullException("Status is required"),
                    Date = Convert.ToDateTime(ordersReader["Date"]),
                });
            }
            ordersReader.Close();

            // get products
            SqlCommand selectProductCommand = new(_selectProductsSql, connection, transaction);
            SqlDataReader productReader = selectProductCommand.ExecuteReader();
            while (productReader.Read())
            {
                products.Add(new()
                {
                    Id = Convert.ToInt32(productReader["Id"]),
                    Description = Convert.ToString(productReader["Description"]) ?? throw new DaoException("Description cannot be NULL"),
                    Name = Convert.ToString(productReader["Name"]) ?? throw new DaoException("Name cannot be NULL"),
                    Price = Convert.ToDecimal(productReader["Price"])
                });
            }
            productReader.Close();

            // get orderlines
            foreach (Order order in orders)
            {
                SqlCommand selectOrderlinesCommand = new(_selectOrderlinesSql, connection, transaction);
                selectOrderlinesCommand.Parameters.AddWithValue("orderId", order.Id);

                SqlDataReader orderlinesReader = selectOrderlinesCommand.ExecuteReader();
                while (orderlinesReader.Read())
                {
                    order.Orderlines.Add(new()
                    {
                        Quantity = Convert.ToInt32(orderlinesReader["Quantity"]),
                        Item = products.Single(p => p.Id == Convert.ToInt32(orderlinesReader["ProductId"]))
                    });
                }
                orderlinesReader.Close();
            }
        }
        catch (SqlException)
        {
            transaction.Rollback();
        }

        transaction.Commit();

        return orders;
    }

    public bool Update(Order entity)
    {
        using SqlConnection connection = DataContext.Open();
        using SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);

        try
        {
            // Update order
            SqlCommand updateCommand = new(_updateOrderSql, connection, transaction);
            updateCommand.Parameters.AddWithValue("id", entity.Id);
            updateCommand.Parameters.AddWithValue("status", entity.Status);

            int result = updateCommand.ExecuteNonQuery();

            // Deleting old orderlines
            SqlCommand deleteOrderlinesCommand = new(_deleteOrderlinesSql, connection, transaction);
            deleteOrderlinesCommand.Parameters.AddWithValue("orderId", entity.Id);

            deleteOrderlinesCommand.ExecuteNonQuery();

            // Insert new orderlines
            foreach (Orderline orderline in entity.Orderlines)
            {
                SqlCommand insertOrderlineCommand = new(_insertOrderlineSql, connection, transaction);
                insertOrderlineCommand.Parameters.AddWithValue("orderId", entity.Id);
                insertOrderlineCommand.Parameters.AddWithValue("productId", orderline.Item.Id);
                insertOrderlineCommand.Parameters.AddWithValue("@quantity", orderline.Quantity);
                insertOrderlineCommand.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {            
            transaction.Rollback();
            return false;
        }
        transaction.Commit();
        return true;
    }

    public bool Delete(Order entity)
    {
        throw new NotImplementedException();
    }
}
