using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebAppCore.BLL.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizePolicyAttribute : AuthorizeAttribute
    {
        public AuthorizePolicyAttribute(params object[] polisies)
        {
            if (polisies.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("polisies");

            Policy = string.Join(",", polisies.Select(r => Enum.GetName(r.GetType(), r)));
        }
    }
}