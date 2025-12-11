using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using Models;


namespace SlashCommands
{
    internal class MainCommands : ApplicationCommandModule
    {
        [SlashCommand("GetTrack", "Ищет треки по названию и исполнителю")]
        public async Task GetTrackAsync(InteractionContext ctx, [Option("Track", "Название трека")] string track, [Option("Artist", "Имя исполнителя")] string artist)
        {
            await ctx.DeferAsync();
            TracksObject resp;

            using (HttpClient client = new())
            {
                HttpRequestMessage request = new(HttpMethod.Get, Program.Url + $"GetTrackByName?track={track}&artist={artist}");
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<TracksObject>(json);
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = $"Результаты для трека \"{track}\" от \"{artist}\"",
                Color = DiscordColor.Azure
            };

            List<DiscordSelectComponentOption> optionList = new();

            foreach (var t in resp.tracks.items)
            {
                optionList.Add(new($"{t.name} {t.artists[0].name}", $"{t.id}"));
                embed.AddField(t.name, $"Альбом: {t.album.name} Исполнитель: {t.artists[0].name}");
            }
            DiscordSelectComponent dropDown = new($"dropDown_select_track", "Выберите произведение...", optionList.AsEnumerable());

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).AddComponents(dropDown));
        }

        [SlashCommand("GetAlbum", "Ищет альбом по названию и исполнителю")]
        public async Task GetAlbumAsync(InteractionContext ctx, [Option("Album", "Название альбома")] string album, [Option("Artist", "Имя исполнителя")] string artist)
        {
            await ctx.DeferAsync();
            AlbumObject resp;

            using (HttpClient client = new())
            {
                HttpRequestMessage request = new(HttpMethod.Get, Program.Url + $"GetAlbumByName?album={album}&artist={artist}");
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<AlbumObject>(json);
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = $"Альбом \"{album}\" от \"{artist}\"",
                Color = DiscordColor.Azure
            };

            List<DiscordSelectComponentOption> optionList = new();

            foreach (var t in resp.albums.items)
            {
                optionList.Add(new($"{t.name} {t.artists[0].name}", $"{t.id}"));
                embed.AddField(t.name, $"Альбом: {t.name} Исполнитель: {t.artists[0].name}");
            }
            DiscordSelectComponent dropDown = new($"dropDown_select_album", "Выберите альбом...", optionList.AsEnumerable());

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).AddComponents(dropDown));
        }

        [SlashCommand("GetArtist", "Ищет исполнителя по имени")]
        public async Task GetArtistAsync(InteractionContext ctx, [Option("Name", "Имя исполнителя")] string name)
        {
            await ctx.DeferAsync();
            ArtistObject resp;

            using (HttpClient client = new())
            {
                HttpRequestMessage request = new(HttpMethod.Get, Program.Url + $"GetArtistByName?name={name}");
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ArtistObject>(json);
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = $"Исполнитель {name}",
                Color = DiscordColor.Azure
            };

            List<DiscordSelectComponentOption> optionList = new();

            foreach (var t in resp.artists.items)
            {
                optionList.Add(new($"{t.name}", $"{t.id}"));
                embed.AddField(t.name, $"Популярность: {t.popularity}");
            }
            DiscordSelectComponent dropDown = new($"dropDown_select_artist", "Выберите исполнителя...", optionList.AsEnumerable());

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).AddComponents(dropDown));
        }
    }
}