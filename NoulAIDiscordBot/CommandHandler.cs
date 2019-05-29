using System.Collections.Generic;

namespace NoulAIBotNetCore.Command
{
    public interface NoulCommand
    {
        string GetCommandName();
        string GetCommandDescription();
    }
}
