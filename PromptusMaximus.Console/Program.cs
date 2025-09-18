using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PromptusMaximus.Core.Configuration;
using PromptusMaximus.Core.Security;
using PromptusMaximus.Console.Commands;
using PromptusMaximus.Console.Utilities;
using PromptusMaximus.Console.Services;
using GitHubModel.Client.Services;

ConsoleUtility.WriteApplicationBanner();

var serviceCollection = new ServiceCollection();
serviceCollection.TryAddSingleton<SessionManager>();
serviceCollection.TryAddSingleton<GitHubModelsService>();
serviceCollection.TryAddSingleton<GitHubModelsClient>();

// Build the service provider
var serviceProvider = serviceCollection.BuildServiceProvider();

var rootCommand = new RootCommand("Promputs Maximus");

rootCommand.Subcommands.Add(new SetCommand(serviceProvider));
rootCommand.Subcommands.Add(new TranslateCommand(serviceProvider));
rootCommand.Subcommands.Add(new ModelsCommand(serviceProvider));

ParseResult parseResult = rootCommand.Parse(args);
return await parseResult.InvokeAsync();
