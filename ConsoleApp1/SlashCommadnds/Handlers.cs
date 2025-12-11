using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Models;
using Newtonsoft.Json;

namespace SlashCommands
{
    public static class Handlers
    {
        public static async Task HandlerDropDownList(DiscordClient sender, ComponentInteractionCreateEventArgs e, string type)
        {
            try
            {
                if (!type.StartsWith("select"))
                {
                    await e.Interaction.CreateResponseAsync(
                        InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                                .WithColor(DiscordColor.Red)
                                .WithTitle("Ошибка")
                                .WithDescription("Неизвестный тип действия")));
                    return;
                }

                var selectedValue = e.Interaction.Data.Values.FirstOrDefault();

                if (string.IsNullOrEmpty(selectedValue))
                {
                    await e.Interaction.CreateResponseAsync(
                        InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                                .WithColor(DiscordColor.Azure)
                                .WithTitle("Ошибка")
                                .WithDescription("Значение не выбрано.")));
                    return;
                }

                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

                if (e.Interaction.Data.CustomId.Contains("track"))
                {
                    Track track;

                    using (HttpClient client = new())
                    {
                        var url = Program.Url + $"GetTrackById?id={selectedValue}";
                        Console.WriteLine(url);
                        var resp = await client.GetStringAsync(url);
                        track = JsonConvert.DeserializeObject<Track>(resp);
                    }

                    var embed = new DiscordEmbedBuilder()
                        .WithTitle($"🎵 {track.name}")
                        .WithDescription($"Исполнитель: {track.artists[0].name}\nАльбом: {track.album.name}")
                        .WithColor(DiscordColor.Azure)
                        .WithThumbnail(track.album.images.FirstOrDefault()?.url ?? "")
                        .AddField("Популярность", track.popularity.ToString(), true)
                        .AddField("Ссылка на Spotify", $"[Открыть]({track.external_urls.spotify})");

                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                }
                else if (e.Interaction.Data.CustomId.Contains("album"))
                {
                    Album album;

                    using (HttpClient client = new())
                    {
                        var url = Program.Url + $"GetAlbumById?id={selectedValue}";

                        Console.WriteLine(url);
                        var resp = await client.GetStringAsync(url);
                        album = JsonConvert.DeserializeObject<Album>(resp);
                    }

                    var embed = new DiscordEmbedBuilder()
                        .WithTitle($"💿 {album.name}")
                        .WithDescription($"Исполнитель: {album.artists[0].name}")
                        .WithColor(DiscordColor.CornflowerBlue)
                        .WithThumbnail(album.images.FirstOrDefault()?.url ?? "")
                        .AddField("Дата релиза", album.release_date, true)
                        .AddField("Треков", album.total_tracks.ToString(), true)
                        .AddField("Ссылка на Spotify", $"[Открыть]({album.external_urls.spotify})");

                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                }
                else if (e.Interaction.Data.CustomId.Contains("artist"))
                {
                    try
                    {
                        Artist artist;

                        using (HttpClient client = new())
                        {
                            var url = Program.Url + $"GetArtistById?id={selectedValue}";
                            var resp = await client.GetStringAsync(url);
                            artist = JsonConvert.DeserializeObject<Artist>(resp);
                            Console.WriteLine(resp);
                        }

                        var embed = new DiscordEmbedBuilder()
                            .WithTitle($"🎤 {artist.name}")
                            .WithColor(DiscordColor.Blurple)
                            .WithThumbnail(artist.images.FirstOrDefault()?.url ?? "")
                            .AddField("Популярность", artist.popularity.ToString(), true)
                            .AddField("Ссылка на Spotify", $"[Открыть]({artist.external_urls.spotify})");

                        await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    await e.Interaction.CreateResponseAsync(
                        InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                                .WithColor(DiscordColor.Red)
                                .WithTitle("Ошибка")
                                .WithDescription("Неизвестный тип выбора")));
                }
            }
            catch (Exception ex)
            {
                await e.Interaction.CreateResponseAsync(
                    InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder()
                        .WithContent($"Ошибка: {ex.Message}")
                        .AsEphemeral(true));
            }
        }
    }
}
