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
        private ILoginRepository _loginRepository;
        private IUserRepository _userRepository;
        public AuthenticationFilterAttribute(ILoginRepository loginRepository, IUserRepository userRepository)
        {
            _loginRepository = loginRepository;
            _userRepository = userRepository;
        }
        
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            IEnumerable<string> authValues;
            if (!actionContext.Request.Headers.TryGetValues("AuthnToken", out authValues))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            Login login = await _loginRepository.SelectByKey(authValues.First()).ConfigureAwait(false);
            if (login == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }   

            User user = await _userRepository.SelectByKey(login.UserName).ConfigureAwait(false);
            var currenController = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            //NEEDS REWORK MAYBE OR ADDING SAME FOR LINKS AND INVITATIONS
            if (currenController == "Users")
            {
                ((UsersController)actionContext.ControllerContext.Controller).SetCurrentUser(user);
            }
            else if(currenController == "Groups")
            {
                ((GroupsController)actionContext.ControllerContext.Controller).SetCurrentUser(user);
            }
            else if (currenController == "Logins")
            {
                ((LoginsController)actionContext.ControllerContext.Controller).SetCurrentUser(user);
            }
            else if (currenController == "Links")
            {
                ((LinksController)actionContext.ControllerContext.Controller).SetCurrentUser(user);
            }

            else if (currenController == "Invitations")
            {
                ((InvitationsController)actionContext.ControllerContext.Controller).SetCurrentUser(user);
            }
            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}