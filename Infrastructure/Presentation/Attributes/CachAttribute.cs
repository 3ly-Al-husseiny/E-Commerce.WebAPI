using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services_Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    public class CachAttribute(int duration) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cachService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CachService;

            var cachKeey = GenerateCachKey(context.HttpContext.Request);

            var result = await cachService.GetCachValueAsync(cachKeey);
            if (!string.IsNullOrEmpty(result)) // Changed from IsNullOrEmpty to !IsNullOrEmpty
            {
                // Return Response from cache
                context.Result = new ContentResult()
                {
                    ContentType = "application/json",
                    Content = result,
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            // Execute The Endpoint
            var contextRsult = await next.Invoke();
            if (contextRsult.Result is OkObjectResult okObject)
            {
                await cachService.SetCacheValueAsync(cachKeey, okObject.Value, TimeSpan.FromSeconds(duration));
            }
        }


        private string GenerateCachKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                key.Append($"|{item.Key}-{item.Value}");
            }

            return key.ToString();
        }
    }
}
