using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingAppAPI.Extensions;
using DatingAppAPI.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DatingAppAPI.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }
            else {
                var username = resultContext.HttpContext.User.GetUsersname();

                var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();

                var user = await repo.GetByUsernameAsync(username);

                user.LastActive = DateTime.Now;

                await repo.SaveAllAsync();
            }
        }
    }
}
