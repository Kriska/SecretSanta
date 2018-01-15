using Autofac;
using System.Web.Http;
using System.Reflection;
using Autofac.Integration.WebApi;
using SecretSanta.CrossDomain;
using SecretSanta.Repository;
using SecretSanta.Controllers;

namespace SecretSanta
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // IoC
            var builder = new ContainerBuilder();

            builder.RegisterType<Settings>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<UserRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<LoginRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GroupRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GlobalErrorHandler>().AsWebApiExceptionFilterFor<ApiController>();

            builder.RegisterType<AuthenticationFilterAttribute>().
                AsWebApiActionFilterFor<UsersController>().InstancePerRequest();
            builder.RegisterType<AuthenticationFilterAttribute>().
               AsWebApiActionFilterFor<GroupsController>().InstancePerRequest();
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
                routeTemplate: "api/{controller}/{userName}",
                defaults: new { userName = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "InvitationApi",
                routeTemplate: "api/{controller}/{userName}/{action}"
            );
        }
    }
}
