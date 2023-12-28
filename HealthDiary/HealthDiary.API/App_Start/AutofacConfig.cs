using Autofac;
using Autofac.Integration.WebApi;
using HealthDiary.BusinessLogic;
using HealthDiary.BusinessLogic.Services.Interfaces;
using System.Reflection;
using System.Web.Http;

namespace HealthDiary.API.App_Start
{
    public class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}