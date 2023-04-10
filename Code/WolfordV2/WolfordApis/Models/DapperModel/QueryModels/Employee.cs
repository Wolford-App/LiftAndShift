using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolfordApis.Models.DapperModel.QueryModels
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Salary { get; set; }

        public Employee() { }
    }

    public class EmployeeId : Employee
    {
        public int Id { get; set; }

        public EmployeeId() { }
    }
}