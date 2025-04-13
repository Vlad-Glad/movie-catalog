using CatalogDomain.Model;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace CatalogInfrastructure.Services
{
    public class MovieExportService : IExportService<Movie>
    {
        private static readonly IReadOnlyList<string> HeaderNames =
            new string[]
            {
                "Title",
                "Year",
                "Description",
                "MovieLength",
                "Poster",
                "Genres",
                "Actors",
                "Directors"
            };

        private readonly DbCatalogContext _catalogContext;

        public MovieExportService(DbCatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
            {
                worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
            }
            worksheet.Row(1).Style.Font.Bold = true;
        }

        private async Task WriteMovieAsync(IXLWorksheet worksheet, Movie movie, int rowIndex, CancellationToken cancellationToken)
        {
            var columnIndex = 1;

            worksheet.Cell(rowIndex, columnIndex++).Value = movie.Title;
            worksheet.Cell(rowIndex, columnIndex++).Value = movie.Year;
            worksheet.Cell(rowIndex, columnIndex++).Value = movie.Description;
            worksheet.Cell(rowIndex, columnIndex++).Value = movie.MovieLength;
            worksheet.Cell(rowIndex, columnIndex++).Value = movie.Poster;

            var genres = await _catalogContext.MovieGenres
                .Where(g => g.MovieId == movie.Id)
                .Include(mg => mg.Genre)
                .Select(mg => mg.Genre.GenreName)
                .Distinct()
                .ToListAsync(cancellationToken);

            worksheet.Cell(rowIndex, columnIndex++).Value = string.Join(", ", genres);

            var actors = await _catalogContext.MovieCasts
                .Where(g => g.MovieId == movie.Id)
                .Include(mg => mg.Actor)
                .Select(mg => mg.Actor.FullName)
                .Distinct()
                .ToListAsync(cancellationToken);

            worksheet.Cell(rowIndex, columnIndex++).Value = string.Join(", ", actors);

            var directors = await _catalogContext.DirectedBies
                .Where(g => g.MovieId == movie.Id)
                .Include(mg => mg.Director)
                .Select(mg => mg.Director.FullName)
                .Distinct()
                .ToListAsync(cancellationToken);

            worksheet.Cell(rowIndex, columnIndex++).Value = string.Join(", ", directors);
        }

        private async Task WriteMoviesAsync(IXLWorksheet worksheet, ICollection<Movie> movies, CancellationToken cancellationToken)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;

            foreach (var movie in movies)
            {
                await WriteMovieAsync(worksheet, movie, rowIndex++, cancellationToken);
            }
        }

        private async Task WriteGenresAsync(XLWorkbook workbook, ICollection<Genre> genres, CancellationToken cancellationToken)
        {
            foreach (var genre in genres)
            {
                if (genre is not null)
                {
                    var worksheet = workbook.Worksheets.Add(genre.GenreName);

                    var moviesWithGenre = await _catalogContext.MovieGenres
                        .Where(mg => mg.GenreId == genre.Id)
                        .Include(mg => mg.Movie)
                        .Select(mg => mg.Movie)
                        .Distinct()
                        .ToListAsync(cancellationToken);

                    await WriteMoviesAsync(worksheet, moviesWithGenre, cancellationToken);
                }
            }
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
                throw new ArgumentException("Input stream is not writable");

            var genres = await _catalogContext.Genres.ToListAsync(cancellationToken);
            var workbook = new XLWorkbook();

            await WriteGenresAsync(workbook, genres, cancellationToken);

            workbook.SaveAs(stream);
        }
    }
}
