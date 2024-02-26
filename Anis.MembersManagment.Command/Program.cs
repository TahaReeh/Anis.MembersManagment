using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Extensions;
using Anis.MembersManagment.Command.Infrastructure.MessageBus;
using Anis.MembersManagment.Command.Infrastructure.Persistence;
using Anis.MembersManagment.Command.Infrastructure.Persistence.DbInitializers;
using Anis.MembersManagment.Command.GrpcServices;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Anis.MembersManagment.Command.Services;


Log.Logger = LoggerServiceBuilder.Build();  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpcWithValidators();
builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddSingleton(new ServiceBusClient(
    builder.Configuration.GetConnectionString("ServiceBus")));
builder.Services.AddSingleton<ServiceBusPublisher>();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<MembersService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.SeedDatabase();

app.Run();

public partial class Program { }
