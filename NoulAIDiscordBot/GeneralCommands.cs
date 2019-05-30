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
            await ReplyAsync("Hey! I'm **NoulAI**, a bot written by a beginner. Why do I exist? I don't know. But what I do know is that " +
                    "I'm being actively maintained! At least, that's what I'm being told to say for now." +
                "\n\nYou can look at all of the commands I know by using **$help**. Yes, I take commands. Like any good bot would." +
                "\n\nIf you want more info, try doing `$info (b/bot)` for more information on me, or `$info (a/author)` for more info on " +
                    "my author!");
        }

        [Command(CommandName)]
        public async Task DisplayInfo(string additional)
        {
            switch (additional)
            {
                case "b":
                case "bot":
                    // TODO: Marking this place so I remember to change it when I add features
                    await ReplyAsync("My name, **NoulAI**, is actually influenced by a character from a story that my author stopped developing a while ago. " +
                            "The character's name is Nouli, pronounced like *\"newly\"*. My name is said like *\"new eye\"*. We're different, see?" +
                        "\n\nI guess you can say that I'm going to be something like a game bot for you here. Planned to be added is Uno, for more friendship " +
                            "demolition, and probably Cards Against Humanity. Perhaps I can take a note from the DiscordRPG bot and make an RPG myself. Hm..." +
                        "\n\nIf you have anything you wanna add, go ahead and raise hell over at <https://github.com/MusicScore/NoulAI-Discord-Bot>. We're " +
                            "always looking for more things to add so that my author can improve his coding skills!");
                    break;
                case "a":
                case "author":
                    await ReplyAsync("My author goes by many names online. I think his GitHub name is MusicScore. Should be easy enough to find." +
                        "\n\nI'm legally obliged to not reveal anything personal, which... is pretty much everything else. Damn, I hate nondisclosure agreements. " +
                            "We could have had a juicy story going :<");
                    break;
                default:
                    await ReplyAsync("I don't know what you mean by that! Quick, ask me something I understand, before they come!");
                    break;
            }
        }

        [Command(CommandName)]
        public async Task DisplayInfo(string additional, [Remainder] string input)
        {
            await DisplayInfo(additional);
        }
    }
}