using Xceed.Words.NET;
using Microsoft.EntityFrameworkCore;

namespace CatalogInfrastructure.Services
{
    public class WordReportService : IReportService
    {
        private readonly DbCatalogContext _context;
        public WordReportService(DbCatalogContext context)
        {
            _context = context;
        }

        public async Task<MemoryStream> GenerateSiteStatisticsReportAsync(CancellationToken cancellationToken = default)
        {
            var movieCount = await _context.Movies.CountAsync(cancellationToken);
            var userCount = await _context.Users.CountAsync(cancellationToken); 
            var genreCounts = await _context.Genres
                .Include(g => g.MovieGenres)
                .Select(g => new { g.GenreName, MovieCount = g.MovieGenres.Count })
                .ToListAsync(cancellationToken);

            var stream = new MemoryStream();
            using (var doc = DocX.Create(stream))
            {
                doc.InsertParagraph("General Catalog Status Report")
                    .FontSize(18).Bold().SpacingAfter(20);

                doc.InsertParagraph($"Total number of movies: {movieCount}");
                doc.InsertParagraph($"Number of registered users: {userCount}");
                doc.InsertParagraph("Number of movies by genre:").SpacingAfter(10);

                foreach (var genre in genreCounts)
                {
                    doc.InsertParagraph($"• {genre.GenreName}: {genre.MovieCount}");
                }

                doc.Save();
            }

            stream.Position = 0;
            return stream;
        }
    }
}
