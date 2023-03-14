using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Web.Http;
using WolfordApis.Models.DapperModel;
using WolfordApis.Models.DapperModel.Interfaces;
using WolfordApis.Models.DapperModel.QueryModels;
using WolfordApis.Models.EmployeeModel;

namespace WolforeApis.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IReadModel _readModel;

        public EmployeeController(IReadModel readModel)
        {
            _readModel = readModel;

        }
   

        //SELECT
        public IActionResult Get()
        {
            var response = this._readModel.GetAllEmployees(QueryTypeEnum.Select);
            return new OkObjectResult(response);
        }

        public IActionResult Get(int id)
        {
            var response = (this._readModel.GetEmployeeFromId(QueryTypeEnum.Select, id));
            if (response.Count() < 1)
                return new NotFoundObjectResult(new { ErrorMessage = $"404: Cant not find employee with id={id}" });
            return new OkObjectResult(response.FirstOrDefault());
        }
        //INSERT
        public IActionResult Post([Microsoft.AspNetCore.Mvc.FromBody] Employee newEmployee)
        {
            ColumnsAndValuesPattern rightPattern = new ColumnsAndValuesPattern().GetPatterns(newEmployee);
            this._readModel.InsertEmployee(QueryTypeEnum.Insert, rightPattern.Columns, rightPattern.Values);
            return new OkObjectResult("200: OK");
        }

        //UPDATE
        public IActionResult Put([Microsoft.AspNetCore.Mvc.FromBody] EmployeeId updateEmployee)
        {
            ColumnsAndValuesPattern rightPattern = new ColumnsAndValuesPattern().GetPatterns(updateEmployee);
            this._readModel.UpdateEmployee(QueryTypeEnum.Update, rightPattern.ColumnsEqValues, updateEmployee.Id);
            return new OkObjectResult("200:Ok");
        }

        //DELETE 

        public IActionResult Delete(int id)
        {
            ;
            this._readModel.DeleteEmployee(QueryTypeEnum.Delete, id);
            return new OkObjectResult("200:Ok"); ;
        }





    }
}
