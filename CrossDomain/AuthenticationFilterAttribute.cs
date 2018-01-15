using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using SecretSanta.Entities;
using SecretSanta.Repository;
using SecretSanta.Controllers;

namespace SecretSanta.CrossDomain
{
    public class AuthenticationFilterAttribute : ActionFilterAttribute, IAutofacActionFilter
    {
        private readonly IRepository<Login,string> _loginRepository;

        public AuthenticationFilterAttribute(IRepository<Login, string> loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            IEnumerable<string> authValues;
            if (!actionContext.Request.Headers.TryGetValues("AuthnToken", out authValues))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            Login login = await _loginRepository.SelectByToken(authValues.First());
            if (login == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }   

            User user = await _loginRepository.GetUserByLogin(login);
            var currenController = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            if(currenController == "Users")
            {
                ((UsersController)actionContext.ControllerContext.Controller).SetCurrentUser(user);
            }
            else
            {
                ((GroupsController)actionContext.ControllerContext.Controller).SetCurrentUser(user);
            }
            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}