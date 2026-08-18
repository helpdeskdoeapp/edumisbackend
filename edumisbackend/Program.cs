using edumis.DataAccess;
using edumis.DataAccess.IRepositories;
using edumis.DataAccess.Repositories;
using edumisbackend.Helpers;
using edumisbackend.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using edumisbackend.Common;
using Npgsql;
using Scalar.AspNetCore;


// Disable inotify FileSystemWatcher to prevent hitting Linux inotify limits in container environments (e.g. Render)
Environment.SetEnvironmentVariable("DOTNET_USE_POLLING_FILE_WATCHER", "true");
Environment.SetEnvironmentVariable("DOTNET_SYSTEM_IO_DISABLEFILEWATCHING", "true");


var builder = WebApplication.CreateBuilder(args);


// Bind dynamically to PORT env var if provided (Render, Cloud Run, Heroku, etc.)
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// Forwarded headers for reverse proxies (Render, NGINX, Load Balancers)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Suppress automatic 400 response on model validation failure
builder.Services.Configure<ApiBehaviorOptions>(option =>
{
    option.SuppressModelStateInvalidFilter = true;
});

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false );
builder.Services.AddTransient<GlobalExceptionHandler>();//register the exception handling middleware

// Add services to the container.
var rawConnectionString = builder.Configuration.GetConnectionString("edumisConStr")
    ?? builder.Configuration["DATABASE_URL"]
    ?? string.Empty;

var effectiveConnectionString = FormatPostgresConnectionString(rawConnectionString);

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(effectiveConnectionString));



builder.Services.AddScoped<SingleFileUpload>();
builder.Services.AddScoped<SmcFileUploadHelper>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

if(!builder.Environment.IsProduction() && !builder.Environment.IsStaging()) 
    builder.Services.AddScoped<IOtpService, OtpService>();
else builder.Services.AddScoped<IOtpService, OtpService>();

builder.Services.AddScoped<TokenHelper>();
builder.Services.AddAutoMapper(_=>{},AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDirectoryBrowser();

//Add JWT Token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme= JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWTAuth:ValidIssuer"],
        ValidAudience = builder.Configuration["JWTAuth:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTAuth:Secret"])),
        ClockSkew = TimeSpan.Zero
    };
    
    options.Events = new JwtBearerEvents {
        OnChallenge = context => {
            if (context.AuthenticateFailure is SecurityTokenExpiredException)
                context.Response.Headers["X-Token-Expired"] = "true";
            return Task.CompletedTask;
        }
    };

});

builder.Services.AddAuthorization();

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); } );
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Education MIS API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // option.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {
    //     {
    //         new OpenApiSecurityScheme
    //         {
    //             Reference = new OpenApiReference
    //             {
    //                 Type = ReferenceType.SecurityScheme,
    //                 Id = "Bearer",
    //             }
    //         },
    //         Array.Empty<string>()
    //     }
    // });
    
    option.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// CORS Policy
var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "https://localhost:3000", "http://localhost:3000", "https://education.delhi.gov.in" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Only if you're sending cookies or auth headers
    });
});

var app = builder.Build();

app.UseForwardedHeaders();
app.UseMiddleware<SecurityHeadersMiddleware>();
if (!app.Environment.IsDevelopment())
    app.UseMiddleware<GlobalExceptionHandler>();//invoke the exception haldler middleware

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Education MIS API v1");
    });
    app.MapOpenApi();
    app.MapScalarApiReference(o => {
        o.Theme = ScalarTheme.Saturn;
    });
    app.MapGet("/", () => Results.Redirect("/scalar"));
}

// Enable CORS
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["UploadPath"] ?? "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

app.UseAuthentication();
if(!app.Environment.IsDevelopment())
    app.UseMiddleware<CsrfProtectionMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();

static string FormatPostgresConnectionString(string? connectionString)
{
    if (string.IsNullOrWhiteSpace(connectionString))
        return string.Empty;

    if (connectionString.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
        connectionString.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;
        var host = uri.Host;
        var port = uri.Port > 0 ? uri.Port : 5432;
        var database = uri.AbsolutePath.TrimStart('/');

        var npgsqlBuilder = new Npgsql.NpgsqlConnectionStringBuilder
        {
            Host = host,
            Port = port,
            Database = database,
            Username = username,
            Password = password,
            SslMode = Npgsql.SslMode.Prefer,
            TrustServerCertificate = true
        };

        return npgsqlBuilder.ConnectionString;
    }

    return connectionString;
}


