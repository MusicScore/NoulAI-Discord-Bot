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
        private const string CommandName = "help";
        private const string CommandDescription = "Displays all help information for any commands recognized by NoulAI.";

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
            await ReplyAsync("Hi! I'm NoulAI, a(n un)friendly bot that's (not) ready to assist! Here's a list of commands:");

            StringBuilder strBuild = new StringBuilder();
            foreach (string nCmd in NoulAIDiscordBot.GetNoulCommands().Keys)
            {
                strBuild.Append("\n__**" + nCmd + "**__:: *" + NoulAIDiscordBot.GetNoulCommands().GetValueOrDefault(nCmd).GetCommandDescription() + "*");
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

    public class InfoCommand : ModuleBase<SocketCommandContext>, NoulCommand
    {
        private const string CommandName = "info";
        private const string CommandDescription = "Displays basic information about NoulAI.";

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
        public async Task DisplayInfo()
        {
            await ReplyAsync("Hey! This command isn't done yet! But maybe <@132178918642810881> will fix that soon >:(");
        }
    }
}