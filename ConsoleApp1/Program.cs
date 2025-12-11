using SlashCommands;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
class Program
{
    private static DiscordClient Client { get; set; }
    private static SlashCommandsExtension Commands { get; set; }
    public static string Url = "http://localhost:5075/api/Main/";
    private static async Task Main()
    {
        
        DiscordConfiguration discordConfig = new()
        {
            Intents = DiscordIntents.All,
            Token = "",
            TokenType = TokenType.Bot,
            AutoReconnect = true
        };
        Client = new(discordConfig);
        Client.Ready += ClientReady;

        Commands = Client.UseSlashCommands();
        Commands.RegisterCommands<MainCommands>();

        Client.ComponentInteractionCreated += async (s, e) =>
        {
            if (e.Id.StartsWith("dropDown"))
            {
                string type = e.Id.Replace("dropDown_", "");
                Handlers.HandlerDropDownList(s, e, type);
            }
        };

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }
    private static Task ClientReady(DiscordClient sender, ReadyEventArgs args)
    {
        return Task.CompletedTask;
    }
}