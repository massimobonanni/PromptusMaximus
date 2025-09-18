// Comandi
// set credential per aggiungere il github token
// set model per scegliere il modello
// set languiage per settare la lingua
// prompt per rispondere
using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PromptusMaximus.Core.Configuration;
using PromptusMaximus.Core.Security;
using PromptusMaximus.Console.Commands;
using PromptusMaximus.Console.Utilities;

ConsoleUtility.WriteApplicationBanner();

var serviceCollection = new ServiceCollection();
serviceCollection.TryAddSingleton<SessionManager>();

// Build the service provider
var serviceProvider = serviceCollection.BuildServiceProvider();

var rootCommand = new RootCommand("Promputs Maximus");

rootCommand.Subcommands.Add(new SetCommand(serviceProvider));
//rootCommand.AddCommand(new UpdateProductStockQuantityCommand(serviceProvider));

ParseResult parseResult = rootCommand.Parse(args);
return await parseResult.InvokeAsync();
