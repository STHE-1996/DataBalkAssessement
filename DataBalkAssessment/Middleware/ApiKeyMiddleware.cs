using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DataBalkAssessment.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataBalkAssessment.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Resolve ApplicationDbContext from the request scope
            var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();

            // Retrieve API Key from headers (ensure we get the first value if there are multiple)
            if (!context.Request.Headers.TryGetValue("ApiKey", out var providedApiKey) || string.IsNullOrEmpty(providedApiKey.FirstOrDefault()))
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("API key is required.");
                return;
            }

            // Use the first API key (in case multiple values exist for the header)
            var apiKey = providedApiKey.FirstOrDefault();

            // Validate API key against stored keys in the database
            var user = await dbContext.users.FirstOrDefaultAsync(u => u.ApiKey == apiKey);
            if (user == null)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid API key.");
                return;
            }

            // Proceed to next middleware or endpoint
            await _next(context);
        }
    }
}
