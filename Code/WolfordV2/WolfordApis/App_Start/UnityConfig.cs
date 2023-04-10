using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.WebApi;
using WolfordApis.Models.DapperModel;
using WolfordApis.Models.DapperModel.Interfaces;
using WolfordApis.Models.DapperModel.QueryExecutor;

namespace WolfordApis.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer().EnableDiagnostic();
            container.RegisterType<IQueryExecutor, QueryExecutor>();
            var queryExecutor = container.Resolve<IQueryExecutor>();

            container.RegisterType<IReadModel, ReadModel>(new InjectionConstructor(ConfigurationManager
                .ConnectionStrings["WolfordEmployeeConnectionString"].ConnectionString
                , queryExecutor));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}