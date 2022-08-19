using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Mosaico.CommandLine.Base;
using Mosaico.Secrets.KeyVault.Extensions;

namespace Mosaico.Tools.CommandLine
{
    internal class Program
    {
        private static Task<int> Main(string[] args)
        {
            Console.WriteLine($"Starting command line. Arguments : {args}");

            var app = new CommandLineApplication();

            var environment = args != null && args.Contains("--prod")
                ? Constants.Environments.Production
                : Constants.Environments.Development;

            Console.WriteLine($"Environment - {environment}");

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .AddJsonFile("config/appsettings.json", true, false);

            if (environment == Constants.Environments.Development)
                configBuilder.AddJsonFile("appsettings.Development.json", true, false);

            configBuilder.AddEnvironmentVariables();
            
            if (environment == Constants.Environments.Production)
                configBuilder.AddKeyVaultSecrets();
            
            var config = configBuilder.Build();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new CommandLineModule(config));
            var container = containerBuilder.Build();

            if (args != null && args.Any()) args = args.Where(a => a != "--prod").ToArray();

            app.OnExecute(() => 0);
            foreach (var command in container.Resolve<IEnumerable<ICommand>>())
            {
                var attribute = command.GetType().GetCustomAttribute<CommandAttribute>();
                app.Command(attribute.Name, commandApplication =>
                {
                    foreach (var commandOption in command.Options)
                        commandApplication.Option(commandOption.Pattern, commandOption.Description,
                            CommandOptionType.SingleValue);
                    commandApplication.Description = attribute.Description;
                    commandApplication.HelpOption("-?|-h|--help");
                    commandApplication.OnExecute(async () =>
                    {
                        try
                        {
                            foreach (var commandOption in command.Options)
                            {
                                var option =
                                    commandApplication.Options.FirstOrDefault(
                                        co => co.Template == commandOption.Pattern);
                                if (!string.IsNullOrEmpty(option?.Value()))
                                    commandOption.Value(option.Value());
                            }

                            Console.WriteLine($"Starting execution of {command.GetType().Name}\n");
                            await command.Execute();
                            Console.WriteLine("\nExecution finished");
                            return 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Unhandled error {ex.Message} / {ex.StackTrace}");
                            Console.ReadLine();
                            return -1;
                        }
                    });
                });
            }
            return Task.FromResult(app.Execute(args));
        }
    }
}