using edumisbackend.Common;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace edumisbackend.Middlewares;

public class CsrfProtectionMiddleware(RequestDelegate _next)
{
   public async Task InvokeAsync(HttpContext context)
    {
        var endPoint = context.GetEndpoint();
        //Skip for all allow anonymous endpoints
        if (endPoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            await _next(context);
            return;
        }

        // Skip CSRF for mobile apps
        var clientType = context.Request.Headers["X-Client-Type"].FirstOrDefault();
        if (clientType?.ToLower() == "mobile")
        {
            await _next(context);
            return;
        }

        if (HttpMethods.IsPost(context.Request.Method) ||
            HttpMethods.IsPut(context.Request.Method) ||
            HttpMethods.IsDelete(context.Request.Method))
        {
            var csrfCookie = context.Request.Cookies["csrfToken"]
                 ?? context.Request.Cookies["almCsrfToken"]
                 ?? string.Empty;
            if (string.IsNullOrEmpty(csrfCookie))
            {
                var returnResponseModel = ResponseModel<string>.ServerError("Invalid Request!", StatusCodes.Status403Forbidden);
                var responseMsg = JsonSerializer.Serialize(returnResponseModel);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(responseMsg);                                
                return;
            }                      

            //var csrfHeaderToken = context.Request.Headers["X-CSRF-TOKEN"].FirstOrDefault();
            //if (string.IsNullOrEmpty(csrfCookie) || string.IsNullOrEmpty(csrfHeader) || csrfCookie != csrfHeader)
            //{
            //    context.Response.StatusCode = StatusCodes.Status400BadRequest;
            //    await context.Response.WriteAsync("Invalid CSRF Token");
            //    return;
            //}

            if (!context.Request.Headers.TryGetValue("X-CSRF-TOKEN", out var csrfToken) || csrfToken != csrfCookie) 
            {
                var returnResponseModel = ResponseModel<string>.ServerError("Invalid Request!", StatusCodes.Status403Forbidden);
                var responseMsg = JsonSerializer.Serialize(returnResponseModel);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(responseMsg);
                return;               
            }
        }
        await _next(context);
    }
}
