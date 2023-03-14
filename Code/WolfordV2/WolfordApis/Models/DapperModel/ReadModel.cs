using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WolfordApis.Models.AzureModel;
using WolfordApis.Models.DapperModel.Interfaces;
using WolfordApis.Models.DapperModel.QueryBuilders;
using WolfordApis.Models.DapperModel.QueryModels;

namespace WolfordApis.Models.DapperModel
{
    class ReadModel : IReadModel
    {
        private readonly string _connection = new ManageKeyVault().GetEmployeeConnectionString();
        private readonly IQueryExecutor _queryExecutor;

        public ReadModel(string connection, IQueryExecutor queryExecutor)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(_connection));
            _queryExecutor = queryExecutor;
        }

        public IEnumerable<T> Fetch<T>(SqlBuilder.Template template)
        {
            using (SqlConnection conn = new SqlConnection(this._connection))
            {
                return this._queryExecutor.Query<T>(conn, template.RawSql, template.Parameters);
            }
        }

        public IEnumerable<Employee> GetAllEmployees(QueryTypeEnum queryType)
            => this.Fetch<Employee>(new EmployeeQueryBuilder()
                .WithQueryMode(queryType)
                .GetTemplate());

        public IEnumerable<Employee> GetEmployeeFromId(QueryTypeEnum queryType, int id)
         => this.Fetch<Employee>(new EmployeeQueryBuilder()
             .WithQueryMode(queryType)
             .WithId(id)
             .GetTemplate()
             );
        public IEnumerable<Employee> InsertEmployee(QueryTypeEnum queryType, IEnumerable<string> columns, IEnumerable<string> values)
         => this.Fetch<Employee>(new EmployeeQueryBuilder()
             .WithQueryMode(queryType)
             .WithColumns(columns)
             .WithValues(values)
             .GetTemplate()
             );
        public IEnumerable<Employee> UpdateEmployee(QueryTypeEnum queryType, IEnumerable<string> columnsAndValues, int id)
              => this.Fetch<Employee>(new EmployeeQueryBuilder()
             .WithQueryMode(queryType)
             .WithColumnsEqValues(columnsAndValues)
             .WithId(id)
             .GetTemplate()
             );

        public IEnumerable<Employee> DeleteEmployee(QueryTypeEnum queryType, int id)
          => this.Fetch<Employee>(new EmployeeQueryBuilder()
             .WithQueryMode(queryType)
             .WithId(id)
             .GetTemplate()
             );

    }
}