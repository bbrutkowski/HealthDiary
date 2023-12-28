using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using HealthDiary.BusinessLogic;
using HealthDiary.BusinessLogic.Services.Interfaces;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace HealthDiary.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();

            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
