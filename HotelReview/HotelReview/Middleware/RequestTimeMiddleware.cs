using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HotelReview.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly Stopwatch stopwatch;
        private readonly ILogger logger;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            stopwatch = new Stopwatch();
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            stopwatch.Start();
            await next.Invoke(context);
            stopwatch.Stop();
            var result = stopwatch.ElapsedMilliseconds;
            if (result / 1000 > 4)
            {
                var message = $"Request: [{context.Request.Method}] at {context.Request.Path} took {result} ms";
                logger.LogInformation(message);
            }
        }
    }
}
