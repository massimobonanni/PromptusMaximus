/// <summary>
/// Entry point for the Promptus Maximus console application.
/// This application provides command-line interface for AI model interactions and session management.
/// </summary>
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

/// <summary>
/// Configure dependency injection container with required services.
/// Registers session management, GitHub Models service, and client services as singletons.
/// </summary>
var serviceCollection = new ServiceCollection();
serviceCollection.TryAddSingleton<SessionManager>();
serviceCollection.TryAddSingleton<GitHubModelsService>();
serviceCollection.TryAddSingleton<GitHubModelsClient>();

// Build the service provider to resolve dependencies
var serviceProvider = serviceCollection.BuildServiceProvider();

/// <summary>
/// Create the root command with available subcommands for the CLI application.
/// Supports 'set' for configuration, 'translate' for AI text processing, and 'models' for model management.
/// </summary>
var rootCommand = new RootCommand("Promputs Maximus");

rootCommand.Subcommands.Add(new SetCommand(serviceProvider.GetSessionManager()));
rootCommand.Subcommands.Add(new TranslateCommand(serviceProvider.GetSessionManager(), serviceProvider.GetModelsService()));
rootCommand.Subcommands.Add(new ModelsCommand(serviceProvider.GetSessionManager(), serviceProvider.GetModelsClient()));

/// <summary>
/// Parse the command-line arguments and execute the corresponding command handler.
/// </summary>
ParseResult parseResult = rootCommand.Parse(args);
return await parseResult.InvokeAsync();
