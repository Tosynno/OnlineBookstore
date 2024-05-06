namespace OnlineBookstore.API.Middleware
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandleMiddleware> _logger;

        public ExceptionHandleMiddleware(RequestDelegate next, ILogger<ExceptionHandleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                httpContext.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");
                httpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                // context.Response.Headers.Add("X-Frame-Options", "DENY");
                httpContext.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                httpContext.Response.Headers.Add("Referrer-Policy", "no-referrer");
                //  context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                httpContext.Response.Headers.Add("Feature-Policy", "camera 'none'; geolocation 'none'; microphone 'none'; usb 'none'");
                httpContext.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message + " " + ex.StackTrace);
                await HandleException(ex, httpContext);
            }
        }

        private async Task HandleException(Exception ex, HttpContext httpContext)
        {


            if (ex is InvalidOperationException)
            {
                httpContext.Response.StatusCode = 400; //HTTP status code
                                                       //httpContext.Response.WriteAsync("Invalid operation");
                                                       //httpContext.Response.WriteAsync("Invalid operation");             
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    Message = "Invalid operation",
                    StatusCode = 400,
                    Success = false
                });
            }
            else if (ex is ArgumentException)
            {
                await httpContext.Response.WriteAsync("Invalid argument");
            }
            else
            {
                await httpContext.Response.WriteAsync("Unknown error");
            }


        }
    }
}
