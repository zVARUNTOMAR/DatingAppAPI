using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingAppAPI.Extensions
{
    public static class ClaimsPrincipleExtensions 
    {
        public static string GetUsersname(this ClaimsPrincipal user) {


            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        }
    }
}
