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
            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<LoginRepository>().As<ILoginRepository>().SingleInstance();
            builder.RegisterType<GroupRepository>().As<IGroupRepository>().SingleInstance();
            builder.RegisterType<LinkRepository>().As<ILinkRepository>().SingleInstance();
            builder.RegisterType<InvitationRepository>().As<IInvitationRepository>().SingleInstance();
            builder.RegisterType<GlobalErrorHandler>().AsWebApiExceptionFilterFor<ApiController>();

           //builder.RegisterType<AuthenticationFilterAttribute>().AsWebApiActionFilterFor<UsersController>().InstancePerRequest();
            //builder.RegisterType<AuthenticationFilterAttribute>().AsWebApiActionFilterFor<GroupsController>().InstancePerRequest();
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
        }
    }
}