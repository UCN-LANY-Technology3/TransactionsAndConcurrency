using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Daos;

internal abstract class BaseDao<TConnection>
{
    private readonly IDataContext<TConnection> _dataContext;

    protected BaseDao(IDataContext<TConnection> dataContext)
    {
        _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
    }

    protected IDataContext<TConnection> DataContext => _dataContext;
}
