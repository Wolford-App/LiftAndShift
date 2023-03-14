using Dapper;
using System.Collections.Generic;
using WolfordApis.Models.DapperModel.QueryModels;

namespace WolfordApis.Models.DapperModel.Interfaces
{
    public interface IReadModel
    {
        IEnumerable<T> Fetch<T>(SqlBuilder.Template template);
        IEnumerable<Employee> GetAllEmployees(QueryTypeEnum queryType);

        IEnumerable<Employee> GetEmployeeFromId(QueryTypeEnum queryType, int id);

        IEnumerable<Employee> UpdateEmployee(QueryTypeEnum queryType, IEnumerable<string> columnsAndValues, int id);

        IEnumerable<Employee> InsertEmployee(QueryTypeEnum queryType, IEnumerable<string> columns, IEnumerable<string> values);
        IEnumerable<Employee> DeleteEmployee(QueryTypeEnum queryType, int id);
    }
}