
using DataAccessLayer.Entities;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer;

public static class DaoFactory
{
    public static IDao<TEntity> Create<TEntity>(IDataContext dataContext) where TEntity : class
    {
        if (typeof(IDataContext<SqlConnection>).IsAssignableFrom(dataContext.GetType()))
        {
            return CreateSqlServerDao<TEntity>(dataContext);
        }
        throw new DaoException("DataContext not supported");
    }

    private static IDao<TEntity> CreateSqlServerDao<TEntity>(IDataContext dataContext)
    {
        try
        {
            return typeof(TEntity) switch
            {
                var dao when dao == typeof(Order) => (IDao<TEntity>)new Daos.Sql.OrderDao((IDataContext<SqlConnection>)dataContext),
                var dao when dao == typeof(Product) => (IDao<TEntity>)new Daos.Sql.ProductDao((IDataContext<SqlConnection>)dataContext),
                _ => throw new DaoException("Unknown model type")
            };
        }
        catch (Exception ex)
        {
            throw new DaoException($"A problem occurred creating DAO for {nameof(TEntity)}", ex);
        }
    }

}
