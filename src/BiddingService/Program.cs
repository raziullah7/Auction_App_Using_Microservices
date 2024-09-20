using BiddingService.Consumers;
using BiddingService.Services;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// rabbitmq config
builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
        {
            h.Username(builder.Configuration.GetValue("RabbitMQ:Username", "guest")!);
            h.Password(builder.Configuration.GetValue("RabbitMQ:Password", "guest")!);
        });

        cfg.ConfigureEndpoints(context);
    });
});

// authentication config
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });

// adding automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// add background service
builder.Services.AddHostedService<CheckAuctionFinished>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

await DB.InitAsync("BidDb", MongoClientSettings
    .FromConnectionString(builder.Configuration.GetConnectionString("BidDbConnection")));

app.Run();