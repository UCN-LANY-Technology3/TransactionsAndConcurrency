namespace DataAccessLayer;

public interface IDao<TEntity> 
{
    TEntity Create(TEntity entity);
    IEnumerable<TEntity> Read();
    bool Update(TEntity entity);
    bool Delete(TEntity entity);
}
