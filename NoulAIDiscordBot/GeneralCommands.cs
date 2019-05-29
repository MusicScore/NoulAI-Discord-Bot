using Discord.Commands;
using NoulAIBotNetCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoulAIBotNetCore.Command
{
    public class HelpCommand : ModuleBase<SocketCommandContext>, NoulCommand
    {
        public const string CommandName = "help";
        public const string CommandDescription = "Displays all help information for any commands recognized by NoulAI.";

        public string GetCommandName()
        {
            return CommandName;
        }

        public string GetCommandDescription()
        {
            return CommandDescription;
        }
        
        [Command(CommandName)]
        [Summary(CommandDescription)]
        public async Task DisplayHelp()
        {
            await ReplyAsync("Hi! I'm NoulAI, a(n un)friendly bot that's (not) ready to assist! Here's a list of commands:\n\n");

            StringBuilder strBuild = new StringBuilder();
            foreach (NoulCommand nCmd in NoulAIDiscordBot.GetNoulCommands())
            {
                strBuild.Append("__**" + nCmd.GetCommandName() + "**__:: " + nCmd.GetCommandDescription() + "\n");
                if (strBuild.Length > 1500)
                {
                    await ReplyAsync(strBuild.ToString());
                    strBuild.Clear();
                }
            }

            if (strBuild.Length != 0)
            {
                await ReplyAsync(strBuild.ToString());
            }
        }

        [Command(CommandName)]
        public async Task DisplayHelp([Remainder] string input)
        {
            await DisplayHelp();
        }
    }
}