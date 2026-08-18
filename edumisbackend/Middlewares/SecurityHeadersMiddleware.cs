namespace edumisbackend.Middlewares;

public class SecurityHeadersMiddleware(RequestDelegate next) {
    
    public async Task InvokeAsync(HttpContext context) {
        var headers = context.Response.Headers;

        // Prevent clickjacking
        headers["X-Frame-Options"] = "DENY";

        // Prevent MIME sniffing
        headers["X-Content-Type-Options"] = "nosniff";

        // XSS protection (legacy browsers)
        headers["X-XSS-Protection"] = "1; mode=block";

        // Control referrer info
        headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        // Restrict powerful features
        headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";

        // HTTPS only 
        headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";

        // Content Security Policy — tighten based on your needs
        headers["Content-Security-Policy"] = "default-src 'self'; frame-ancestors 'none';";

        // Remove leaky headers
        headers.Remove("X-Powered-By"); 
        headers.Remove("X-AspNet-Version");
        headers.Remove("Server");

        await next(context);
    }
}