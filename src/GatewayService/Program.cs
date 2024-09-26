using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });

// Adding CORS for cross requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("customPolicy", b =>
    {
        b.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
            .WithOrigins(builder.Configuration["ClientApp"] ?? string.Empty);
    });
});

var app = builder.Build();

// middleware for CORS
app.UseCors();

app.MapReverseProxy();

app.UseAuthentication();
app.UseAuthorization();

app.Run();