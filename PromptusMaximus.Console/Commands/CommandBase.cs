using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using PromptusMaximus.Core.Configuration;
using PromptusMaximus.Core.Interfaces;

namespace PromptusMaximus.Console.Commands
{
    internal abstract class CommandBase : Command
    {

        protected readonly ISessionManager _sessionManager;

        public CommandBase(string name, string description, ISessionManager sessionManager) : base(name, description)
        {
            _sessionManager = sessionManager;
        }
    }

}
