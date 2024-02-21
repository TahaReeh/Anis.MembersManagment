using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Infrastructure.MessageBus;
using Anis.MembersManagment.Command.Infrastructure.Persistence;
using Anis.MembersManagment.Command.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddSingleton(new ServiceBusClient(
    builder.Configuration.GetConnectionString("ServiceBus")));
builder.Services.AddSingleton<ServiceBusPublisher>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<InvitationsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

public partial class Program { }
