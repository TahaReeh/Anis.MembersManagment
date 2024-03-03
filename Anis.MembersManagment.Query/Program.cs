using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.GrpcServices.Interceptors;
using Anis.MembersManagment.Query.Infrastructure.Persistence;
using Anis.MembersManagment.Query.Infrastructure.Persistence.Repositories;
using Anis.MembersManagment.Query.Infrastructure.ServiceBus;
using Anis.MembersManagment.Query.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ApplicationExceptionInterceptor>();
});
builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddSingleton<MembersServiceBus>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHostedService<MembersEventsListner>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<MembersService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

public partial class Program { }