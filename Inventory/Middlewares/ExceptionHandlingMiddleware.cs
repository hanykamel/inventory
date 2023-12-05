using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Inventory.Web.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {

                //ILogger _logger = logger;
                await HandleExceptionAsync(context, ex, logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ExceptionHandlingMiddleware> logger)
        {
            var code = HttpStatusCode.BadRequest; // 400 if unexpected
            string Message = "";
            if (ex is InventoryExceptionBase)
                Message = ex.Message;
            else if (ex is AggregateException)
            {
                foreach (var innerEx in ((System.AggregateException)ex).InnerExceptions)
                {
                    if (innerEx is InventoryExceptionBase)
                        Message += innerEx.Message;
                }
            }
            else if (ex is Exception)
                Message = "خطأ في الخادم الداخلي";
            var result = JsonConvert.SerializeObject(new
            {
                Success = "false",
                Message,
                ExceptionType = "Custom"
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            logger.LogError(Message);
            return context.Response.WriteAsync(result);
        }
    }
}
