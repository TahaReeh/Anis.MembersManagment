using Anis.MembersManagment.Query.ServiceExtensions;
using Anis.MembersManagment.Query.Services;
using Serilog;

Log.Logger = LoggerServiceBuilder.Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpcWithValidators();
builder.Services.AddEntityFramework(builder.Configuration);
builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddServiceBus();
builder.Services.AddHostedServices();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<MembersService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

public partial class Program { }