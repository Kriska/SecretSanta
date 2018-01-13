using Autofac;
using System.Web.Http;
using System.Net.Http;
using System.Reflection;
using Autofac.Integration.WebApi;
using SecretSanta.Repository;
using SecretSanta.CrossDomain;

namespace SecretSanta
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // IoC
            var builder = new ContainerBuilder();

            builder.RegisterType<Settings>().AsImplementedInterfaces().SingleInstance();
           // builder.RegisterType<UserRepository>().As<IRepository<UserRepository, string>().SingleInstance();
           //za loginRepo builder.RegisterType<>().As<ICompetitionRepository>().SingleInstance();
          //  builder.RegisterType<GlobalErrorHandler>().AsWebApiExceptionFilterFor<ApiController>();

            // вече не го слагаме просто като атрибут а го регистрираме тук
            // builder.RegisterType<AuthenticationFilterAttribute>().AsWebApiActionFilterFor<SnailsController>().InstancePerRequest();

            // регистрация на Delegating handler
           // builder.RegisterType<ExampleDelegatingHandler>().As<DelegatingHandler>().InstancePerRequest();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
