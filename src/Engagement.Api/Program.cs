#pragma warning disable CA1506
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json;
using TikTokFeed.Contracts.Auth;
using TikTokFeed.Contracts.Errors;
using TikTokFeed.Contracts.Json;
using TikTokFeed.Engagement.Api.Grpc;
using TikTokFeed.Engagement.Api.Middleware;
using TikTokFeed.Engagement.Api.Services;
using TikTokFeed.Engagement.Application;
using TikTokFeed.Engagement.Application.Abstractions.Services;
using TikTokFeed.Engagement.Infrastructure;
using TikTokFeed.Engagement.Infrastructure.Persistence;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.WithProperty("service", "engagement")
    .WriteTo.Console());

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080, listen => listen.Protocols = HttpProtocols.Http1);
    options.ListenAnyIP(8081, listen => listen.Protocols = HttpProtocols.Http2);
});

builder.Services
    .AddControllers()
    .AddJsonOptions(options => SnakeCaseJson.Apply(options.JsonSerializerOptions));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        string message = string.Join(
            "; ",
            context.ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));
        return new BadRequestObjectResult(ErrorResponse.Create(ErrorCodes.ValidationError, message));
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddGrpc();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

IConfigurationSection jwtSettings = builder.Configuration.GetSection("JwtSettings");
string secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            RoleClaimType = JwtClaimNames.Role,
            NameClaimType = JwtClaimNames.Username,
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async challenge =>
            {
                challenge.HandleResponse();
                if (challenge.Response.HasStarted)
                {
                    return;
                }

                challenge.Response.StatusCode = StatusCodes.Status401Unauthorized;
                challenge.Response.ContentType = "application/json";
                var body = ErrorResponse.Create(ErrorCodes.Unauthorized, "JWT token is missing or invalid");
                await challenge.Response.WriteAsync(JsonSerializer.Serialize(body));
            },
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Engagement Service", Version = "v1" });
    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" },
    };
    options.AddSecurityDefinition("bearerAuth", scheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { [scheme] = Array.Empty<string>() });
});

WebApplication app = builder.Build();

await EnsureDatabaseAsync(app);

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<EngagementGrpcService>();
app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "engagement" }));

await app.RunAsync();

static async Task EnsureDatabaseAsync(WebApplication app)
{
    const int maxAttempts = 10;
    for (int attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            using IServiceScope scope = app.Services.CreateScope();
            EngagementDbContext db = scope.ServiceProvider.GetRequiredService<EngagementDbContext>();
            await db.Database.EnsureCreatedAsync();
            return;
        }
        catch (Exception ex) when (attempt < maxAttempts)
        {
            app.Logger.LogWarning(ex, "Database not ready (attempt {Attempt}/{Max})", attempt, maxAttempts);
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}

public partial class Program
{
    private Program()
    {
    }
}
