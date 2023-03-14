using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WolfordApis.Models.DapperModel.QueryFormatter;
using WolfordApis.Models.DapperModel.QueryModels;

namespace WolfordApis.Models.DapperModel.QueryBuilders
{
     class EmployeeQueryBuilder
    {
        private const string Table = "[dbo].[Employees]";
        private SqlBuilder Builder = new SqlBuilder();
        private QueryReplacer QueryReplacer = new QueryReplacer();
        private QueryTypeEnum queryMode;
        private IEnumerable<string> columns;
        private IEnumerable<string> values;
        private IEnumerable<string> columnsEqValues;
        private int? id;


        private string SelectQuery = $"SELECT [FirstName] AS {nameof(Employee.FirstName)}," +
                                $" [LastName] AS {nameof(Employee.LastName)}," +
                                $" [Salary] AS {nameof(Employee.Salary)}" +
                                $" FROM {Table} /**where**/ ORDER BY [LastName]";

        private string UpdateQuery = $"UPDATE {Table} SET /**columns=values**/ /**where**/";

        private string DeleteQuery = $"DELETE FROM {Table}  /**where**/";

        private string InsertQuery = $"INSERT INTO {Table} /**columns**/ VALUES /**values**/";

        private string GetQuery()
        {
            switch (this.queryMode)
            {
                case QueryTypeEnum.Select:
                    return SelectQuery;
                case QueryTypeEnum.Update:
                    return QueryReplacer.CleanQuery(this.UpdateQuery, this.columnsEqValues, true);
                case QueryTypeEnum.Insert:
                    return QueryReplacer.CleanQuery(this.InsertQuery, this.columns, this.values);
                case QueryTypeEnum.Delete:
                    return DeleteQuery;
                default:
                    throw new InvalidOperationException("Unknown query type.");
            }

        }

        internal EmployeeQueryBuilder WithQueryMode(QueryTypeEnum queryType)
        {
            this.queryMode = queryType;
            return this;
        }

        internal EmployeeQueryBuilder WithColumns(IEnumerable<string> columns)
        {
            this.columns = columns;
            return this;
        }

        internal EmployeeQueryBuilder WithValues(IEnumerable<string> values)
        {
            this.values = values;
            return this;
        }

        internal EmployeeQueryBuilder WithColumnsEqValues(IEnumerable<string> colEqVal)
        {
            this.columnsEqValues = colEqVal;
            return this;
        }

        internal EmployeeQueryBuilder WithId(int Id)
        {
            this.id = Id;
            return this;
        }

        internal SqlBuilder.Template GetTemplate()
        {
            string query = this.GetQuery();
            if (this.id != null)
            {
                Builder.Where("@id=[ID]", new { id = this.id });
            }
            return Builder.AddTemplate(query);

        }
    }
}