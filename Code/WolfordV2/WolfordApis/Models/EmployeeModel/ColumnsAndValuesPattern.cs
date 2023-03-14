using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WolfordApis.Models.DapperModel.QueryModels;

namespace WolfordApis.Models.EmployeeModel
{
    public class ColumnsAndValuesPattern
    {
        public List<string> Columns = new List<string>();
        public List<string> Values = new List<string>();
        public List<string> ColumnsEqValues = new List<string>();
        private readonly string[] ToCovert = new string[] { "Id", "Salary" };

        public ColumnsAndValuesPattern GetPatterns(Employee employee)
        {
            PropertyInfo[] properties = employee.GetType().GetProperties();
            foreach (var column in properties)
            {

                this.Columns.Add(column.Name);
                if (this.ToCovert.Contains(column.Name))
                {
                    this.Values.Add($"###{column.GetValue(employee)}###");
                }
                else
                {
                    this.Values.Add($"{column.GetValue(employee)}");
                }

            }
            return this;
        }

        public ColumnsAndValuesPattern GetPatterns(EmployeeId employee)
        {
            PropertyInfo[] properties = employee.GetType().GetProperties();
            foreach (var column in properties)
            {
                if (column.Name != "Id")
                {
                    if (this.ToCovert.Contains(column.Name))
                    {
                        this.ColumnsEqValues.Add($"{column.Name}={column.GetValue(employee)}");
                    }
                    else
                    {
                        this.ColumnsEqValues.Add($"{column.Name}='{column.GetValue(employee)}'");
                    }

                }

            }
            return this;
        }
    }
}