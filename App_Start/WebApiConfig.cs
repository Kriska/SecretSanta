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
            builder.RegisterType<AuthenticationFilterAttribute>().AsWebApiActionFilterFor<GroupsController>().InstancePerRequest();
            builder.RegisterType<AuthenticationFilterAttribute>().AsWebApiActionFilterFor<InvitationsController>().InstancePerRequest();
            builder.RegisterType<AuthenticationFilterAttribute>().AsWebApiActionFilterFor<LinksController>().InstancePerRequest();
           // builder.RegisterType<AuthenticationFilterAttribute>().AsWebApiActionFilterFor<LoginsController>().InstancePerRequest();

            builder.RegisterType<AuthenticationFilterAttribute>().AsWebApiActionFilterFor<LoginsController>(c => c.Logout());
            builder.RegisterType<AuthenticationFilterAttribute>().AsWebApiActionFilterFor<UsersController>(c => c.GetAllUsers(1,1,null,"A"));
            
            
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
                routeTemplate: "api/{controller}/{userName}/{groupName}",
                defaults: new { userName = RouteParameter.Optional, groupName = RouteParameter.Optional }
            );
        }
    }
}