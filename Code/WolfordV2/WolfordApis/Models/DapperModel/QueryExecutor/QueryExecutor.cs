using Dapper;
using System.Collections.Generic;
using System.Data;
using WolfordApis.Models.DapperModel.Interfaces;

namespace WolfordApis.Models.DapperModel.QueryExecutor
{
    class QueryExecutor : IQueryExecutor
    {
        public IEnumerable<T> Query<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.Query<T>(sql, param);
        }

    }
}