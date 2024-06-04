namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Console.WriteLine(ExportAlbumsInfo(context, 9));
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {

            var albums = context.Albums
                .AsNoTracking()
                .Where(a => a.ProducerId == producerId)
                .Include(a => a.Songs)
                .Select(a => new
                {
                    a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer!.Name,
                    AlbumSongs = a.Songs
                        .Select(s => new
                        {
                            s.Name,
                            s.Price,
                            WriterName = s.Writer.Name
                        })
                        .OrderByDescending(a => a.Name)
                        .ThenBy(s => s.WriterName)
                        .ToList(),
                    a.Price
                })
                .ToList()
                .OrderByDescending(a => a.Price);

            StringBuilder result = new();
            foreach (var album in albums)
            {
                result
                    .AppendLine($"-AlbumName: {album.Name}")
                    .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                    .AppendLine($"-ProducerName: {album.ProducerName}");

                result
                    .AppendLine("-Songs:");

                int counter = 0;
                foreach (var song in album.AlbumSongs)
                {
                    result
                        .AppendLine($"---#{++counter}")
                        .AppendLine($"---SongName: {song.Name}")
                        .AppendLine($"---Price: {song.Price:f2}")
                        .AppendLine($"---Writer: {song.WriterName}");
                }
                result
                    .AppendLine($"-AlbumPrice: {album.Price:f2}");
            }

            return result.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .AsNoTracking()
                .Select(s => new
                {
                    s.Name,
                    WriterName = s.Writer.Name,
                    SongPerformers = s.SongPerformers
                        .Select(sp => new
                        {
                            PerformerName = sp.Performer.FirstName + " " + sp.Performer.LastName,
                        })
                        .OrderBy(p => p.PerformerName)
                        .ToList(),
                    AlbumProducer = s.Album!.Producer!.Name,
                    s.Duration

                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ToList()
                .Where(s => s.Duration.TotalSeconds > duration);

            StringBuilder result = new();

            int counter = 0;
            foreach (var song in songs)
            {
                result
                    .AppendLine($"-Song #{++counter}")
                    .AppendLine($"---SongName: {song.Name}")
                    .AppendLine($"---Writer: {song.WriterName}");

                if (song.SongPerformers.Any())
                {
                    foreach (var performer in song.SongPerformers)
                    {
                        result
                            .AppendLine($"---Performer: {performer.PerformerName}");
                    }
                }

                result
                    .AppendLine($"---AlbumProducer: {song.AlbumProducer}")
                    .AppendLine($"---Duration: {song.Duration}");


            }

            return result.ToString().Trim();
        }
    }
}
