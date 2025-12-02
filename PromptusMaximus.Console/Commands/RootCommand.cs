using Microsoft.Extensions.DependencyInjection;
using PromptusMaximus.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.Text;

namespace PromptusMaximus.Console.Commands
{
    internal class RootCommand : System.CommandLine.RootCommand
    {
        public RootCommand(ServiceProvider serviceProvider) : base("Promptus Maximus")
        {
            this.Description = "The command-line interface for speaking like Caesar and debugging like a gladiator.";

            this.Subcommands.Add(new SetCommand(serviceProvider.GetSessionManager()));
            this.Subcommands.Add(new TranslateCommand(serviceProvider.GetSessionManager(), serviceProvider.GetModelsService()));
            this.Subcommands.Add(new ModelsCommand(serviceProvider.GetSessionManager(), serviceProvider.GetModelsClient()));

            for (int i = 0; i < this.Options.Count; i++)
            {
                if (this.Options[i] is VersionOption defaultVersionOption)
                {
                    defaultVersionOption.Action = new CustomVersionAction();
                }
            }
        }
    }
}
