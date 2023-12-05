using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Inventory.Web.Middlewares
{
    public static class RefreshTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseRefreshToken(
            this IApplicationBuilder builder, int seconds)
        {
            return builder.UseMiddleware<RefreshTokenMiddleware>(seconds);
        }
    }

}

