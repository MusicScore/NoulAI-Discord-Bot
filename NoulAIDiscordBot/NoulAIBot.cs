using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using NoulAIBotNetCore.Configuration;
using NoulAIBotNetCore.Command;
using NoulAIBotNetCore.Utilities;
using YamlDotNet.Serialization.NamingConventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;

namespace NoulAIBotNetCore
{
    public class NoulAIDiscordBot {
        public DiscordSocketClient Client;
        public CommandService Commands;
        public IServiceProvider Services;

        public static NoulAIDiscordBot instance;

        private NoulDiscordSettings Config = null;
        private const string BotLogPrefix = "NoulAIDiscordBot";
        private const string ConfigFile = "config.yml";

        private static Dictionary<string, NoulCommand> CommandList = new Dictionary<string, NoulCommand>();

        public static void Main(string[] args)
        {
            instance = new NoulAIDiscordBot();

            RegisterCommands();
            instance.StartAsync().GetAwaiter().GetResult();
        }

        public async Task StartAsync()
        {
            try
            {
                using (FileStream stream = File.OpenRead(ConfigFile))
                {
                    Config = new YamlConfig(new UnderscoredNamingConvention()).Load<NoulDiscordSettings>(stream);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Could not find the file [config.yml]. Try again!");
                return;
            }

            if (Config == null || Config.Discord == null)
            {
                Console.WriteLine("Could not find \"discord\" key with a valid token in the file [config.yml]!");
                return;
            }

            Client = new DiscordSocketClient();
            Client.MessageReceived += HandleCommandAsync;
            Commands = new CommandService();
            Services = new ServiceCollection().AddSingleton(Client).AddSingleton(Commands).BuildServiceProvider();

            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);

            Client.Log += async (e) =>
            {
                Console.WriteLine($"[{e.Severity}] {e.Source} >> {e.Message}");
                await Task.Delay(0);
            };
            /*
                Since we already have another bot handling user leaves and joins, we don't need this. But
                I'm keeping it here in case I want to use it.

            Client.UserJoined += async (e) =>
            {
                if (!e.IsBot())
                {
                    await e.Guild.DefaultChannel.SendMessageAsync("Welcome to " + e.Guild.Name + ", **" + e.Username + "**!");
                }
            };
            Client.UserLeft += async (e) =>
            {
                if (!e.IsBot())
                {
                    await e.Guild.DefaultChannel.SendMessageAsync("**" + e.Username + "** just left. Bye!");
                }
            };
            */

            await Client.LoginAsync(TokenType.Bot, Config.Discord);
            await Client.StartAsync();
            await Task.Delay(-1);
        }

        private static void RegisterCommands()
        {
            CommandList.Add("help", new HelpCommand());
            CommandList.Add("info", new InfoCommand());
        }
        
        public static Dictionary<string, NoulCommand> GetNoulCommands()
        {
            return CommandList;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine("[" + StringUtilities.Before(msg.ToString(), ' ') + "][" + msg.Source + "/" + msg.Severity + "] :: " + msg.Message );
            return Task.CompletedTask;
        }

        private Task LogInfo(string message)
        {
            Log(new LogMessage(LogSeverity.Info, BotLogPrefix, message, null));
            return Task.CompletedTask;
        }

        private Task LogInfo(string source, string message)
        {
            Log(new LogMessage(LogSeverity.Info, source, message, null));
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            if (socketMessage is SocketUserMessage msg && !msg.Author.IsBot)
            {
                int argPos = 0;

                if (!msg.HasStringPrefix("$", ref argPos) && !msg.HasMentionPrefix(Client.CurrentUser, ref argPos))
                {
                    return;
                }

                ICommandContext context = new SocketCommandContext(Client, msg);
                string cmdName = (msg.HasMentionPrefix(Client.CurrentUser, ref argPos) ? msg.ToString().Split(' ')[1] : msg.ToString().Split(' ')[0].Substring(1)).ToLower();
                string cmdText = StringUtilities.After(msg.ToString(), cmdName, true).ToLower();

                if (Commands.Search(cmdName).IsSuccess)
                {
                    await LogInfo("User (" + msg.Author.Username + ")<" + msg.Author.Id + "> has run command \"$" +
                        cmdName + (cmdText != "" ? " " + cmdText : "") + "\".");

                    if (!CommandList.ContainsKey(cmdName))
                    {
                        await context.Channel.SendMessageAsync("Hey! The command **" + msg.ToString().Split(' ')[0] + "** looks like something I could run, "
                            + "but I can't. Is there another bot that handles that command? I don't know if there are. I'm just a terminal bot, I can't see!");
                        await LogInfo("> > Command failed! Command exists, but is not registered!");
                        return;
                    }
                }

                IResult result = await Commands.ExecuteAsync(context, argPos, Services);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                    await LogInfo("> > Command failed! Reason: " + result.ErrorReason);
                }
            }
        }

        private static string GetConfigString()
        {
            try
            {
                return File.ReadAllText(ConfigFile);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read config.yml. Stacktrace:");
                Console.WriteLine(e);
                return "";
            }
        }
    }
}
