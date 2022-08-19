using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mosaico.Secrets.KeyVault.Extensions;
using Serilog;
using Serilog.Events;

namespace Mosaico.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(LogEventLevel.Information)
                .CreateLogger();

            try
            {
                Log.Information("The identity application is starting up...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("config/appsettings.json", true, false);
                    builder.AddUserSecrets(typeof(Program).Assembly)
                        .AddEnvironmentVariables()
                        .AddKeyVaultSecrets();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureServices(services => services.AddAutofac())
                        .ConfigureLogging((hostingContext, builder) => { builder.ClearProviders(); })
                        .UseStartup<Startup>();
                });
        }
    }
}
