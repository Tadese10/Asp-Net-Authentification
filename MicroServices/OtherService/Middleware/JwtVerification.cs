namespace Middlewares
{
    public class JwtVerification
    {
        private readonly RequestDelegate _next;

        public JwtVerification(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var cookie = context.Request.Cookies["access_cookie"];

                if (cookie is null)
                {
                    // Retournez une réponse Ok
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                var cookieParts = cookie.Split(".JTW");

                var token = cookieParts[0];

                if(token is null)
                {
                    // Retournez une réponse Ok
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                context.Request.Headers.Add("Authorization", "Bearer " + token);
            }

            catch(Exception)
            {
                context.Response.StatusCode = 440;
                return;
            }

            await _next(context);
        }
    }

    public static class JwtVerificationExtension
    {
        public static IApplicationBuilder UseJWTVerification(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtVerification>();
        }
    }
}