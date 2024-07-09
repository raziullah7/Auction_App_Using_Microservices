using AuctionService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container. ////
// add controllers service
builder.Services.AddControllers();
// add DB service
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// add auto-mapper service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// add mass-transit service
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

//// build the app. ////
var app = builder.Build();

//// Configure the HTTP request pipeline. ////
app.UseAuthorization();

app.MapControllers();

// getting seed data
try
{
    DbInitializer.InitDb(app);
}
catch(Exception e)
{
    Console.WriteLine(e);
}

app.Run();
