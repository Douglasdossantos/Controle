using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Controle.Extended
{
    public static class UserExtended
    {
        public static string GetFullName(this IPrincipal user)
        {
            var clain = ((ClaimsIdentity)user.Identity).FindFirst("fullName");
            return clain?.Value;
        }
        public static string GetEmail(this IPrincipal user)
        {
            var clain = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Email);
            return clain?.Value;
        }
        public static string GetNickName(this IPrincipal user)
        {
            var clain = ((ClaimsIdentity)user.Identity).FindFirst("NickName");
            return clain?.Value;
        }
    }
}