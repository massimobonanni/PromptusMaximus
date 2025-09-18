using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using PromptusMaximus.Core.Configuration;

namespace PromptusMaximus.Console.Commands
{
    internal abstract class CommandBase : Command
    {

        protected readonly ServiceProvider _serviceProvider;
        protected readonly SessionManager _sessionManager;

        public CommandBase(string name, string description, ServiceProvider serviceProvider) : base(name, description)
        {
            _serviceProvider = serviceProvider;
            _sessionManager = _serviceProvider.GetRequiredService<SessionManager>();
        }
    }

}
