using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace Middlewares
{
    public class CookieVerification
    {
        private readonly RequestDelegate _next;
        private readonly ICryptService _cryptService;
        private readonly IControllerFactory _controllerFactory;

        public CookieVerification(RequestDelegate next, ICryptService cryptService, IControllerFactory controllerFactory)
        {
            _next = next;
            _cryptService = cryptService;
            _controllerFactory = controllerFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if(!verifySingleCookie(context.Request.Cookies["access_cookie"]!))
                {
                    throw new UnauthorizedAccessException();
                }

                if(!verifySingleCookie(context.Request.Cookies["session_cookie"]!))
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch(Exception)
            {
                context.Response.StatusCode = 440;
                return;
            }

            await _next(context);
        }

  
        private bool verifySingleCookie(string cookie)
        {

            if(cookie is null)
            {
                return false;
            }

            try
            {
                var cookieParts = cookie.Split(".JTW");
                var value = cookieParts[0];
                var signature = cookieParts[1];

                if(!_cryptService.VerifyCookie(value, signature))
                {
                    return false;
                }

                return true;
            }

            catch(Exception)
            {
                return false;
            }
        }
    }

    public static class CookieVerificationExtension
    {
        public static IApplicationBuilder UseCookieVerification(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieVerification>();
        }
    }
}