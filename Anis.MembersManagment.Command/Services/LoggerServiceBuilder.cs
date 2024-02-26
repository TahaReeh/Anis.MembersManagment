using Serilog;

namespace Anis.MembersManagment.Command.Services
{
    public class LoggerServiceBuilder
    {
        public static Serilog.ILogger Build()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var serilogConfiguration = configuration.GetSection("Serilog");
            var appName = serilogConfiguration["AppName"];

            var logger = new LoggerConfiguration()
                .Enrich.WithProperty("Name", appName)
                .ReadFrom.Configuration(configuration);

            logger.WriteTo.Console();

            return logger.CreateLogger();

        }
    }
}
