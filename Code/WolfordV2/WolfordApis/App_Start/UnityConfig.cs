using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.WebApi;
using WolfordApis.Models.AzureModel;
using WolfordApis.Models.DapperModel;
using WolfordApis.Models.DapperModel.Interfaces;
using WolfordApis.Models.DapperModel.QueryExecutor;

namespace WolforeApis
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer().EnableDiagnostic();
            container.RegisterType<IQueryExecutor, QueryExecutor>();
            var queryExecutor = container.Resolve<IQueryExecutor>();

            container.RegisterType<IReadModel, ReadModel>(new InjectionConstructor(new ManageKeyVault().
                  GetEmployeeConnectionString(), queryExecutor));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}