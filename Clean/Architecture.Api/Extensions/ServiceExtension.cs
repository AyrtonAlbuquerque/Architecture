namespace Architecture.Api.Extensions
{
    public static class ServiceExtension
    {
        public static IApplicationBuilder UseBuffering(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });

            return app;
        }

    }
}