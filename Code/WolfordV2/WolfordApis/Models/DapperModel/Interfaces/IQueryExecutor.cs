using System.Collections.Generic;
using System.Data;

namespace WolfordApis.Models.DapperModel.Interfaces
{
    public interface IQueryExecutor
    {

        IEnumerable<T> Query<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}