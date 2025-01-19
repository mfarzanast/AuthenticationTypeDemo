namespace AuthenticationTypes.AuthHandlers
{
    public class ApiKeyHandler
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyHeaderName = "X-Api-Key";
        private const string ApiKey = "MySecretApiKey"; // replace with your actual APIKEy;
        public ApiKeyHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401; //Unauthorized
                await context.Response.WriteAsync("API key is missing");
                return;
            }
            if (!ApiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 403; //Forbidden
                await context.Response.WriteAsync("Invalid API key");
                return;
            }
            await _next(context);  //call the next middleware
        }
    }
}
