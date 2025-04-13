using CatalogDomain.Model;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Composition;
using System.Reflection.Metadata.Ecma335;

namespace CatalogInfrastructure.Services
{
    public class MovieImportService : IImportService<Movie>
    {
        private readonly DbCatalogContext _catalogContext;

        public MovieImportService(DbCatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {

            if (!stream.CanRead)
                throw new ArgumentException("Can't read the data", nameof(stream));

            using (XLWorkbook workbook = new XLWorkbook(stream/*, XLEventTracking.Disabled*/))
            {
                foreach (var worksheet in workbook.Worksheets)
                {
                    var genreName = worksheet.Name;

                    var genre = await _catalogContext.Genres.FirstOrDefaultAsync(gen => gen.GenreName == genreName);

                    if (genre == null)
                    {
                        genre = new Genre { GenreName = genreName };
                        _catalogContext.Genres.Add(genre);
                    }

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        await AddMoviesAsync(row, genre, cancellationToken);
                    }
                }
            }

            await _catalogContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddMoviesAsync(IXLRow row, Genre genre, CancellationToken cancellationToken)
        {
            string title = GetMovieTitle(row);
            int year = GetMovieYear(row);
            int length = GetMovieLength(row);

            int currentYear = DateTime.Now.Year;
            if (year < 1900 || year > currentYear)
                throw new ArgumentException($"Invalid year '{year}' for movie '{title}'. Year must be between 1900 and {currentYear}.");

            if (length <= 0 || length > 10000)
                throw new ArgumentException($"Invalid movie length '{length}' for movie '{title}'. Must be between 1 and 10000 minutes.");

            var existingMovie = await _catalogContext.Movies.FirstOrDefaultAsync(m => m.Title == title && m.Year == year, cancellationToken);

            if (existingMovie != null)
            {
                await AddGenreToMovie(existingMovie, genre, cancellationToken);
                await AddDirectorsAsync(row, existingMovie, cancellationToken);
                await AddActorsAsync(row, existingMovie, cancellationToken);

                //return;
            }

            Movie movie = new Movie
            {
                Title = title,
                Year = year,
                Description = GetMovieDescription(row),
                MovieLength = length,
                Poster = GetMoviePoster(row)
            };

            _catalogContext.Movies.Add(movie);

            await AddGenreToMovie(movie, genre, cancellationToken);
            await AddDirectorsAsync(row, movie, cancellationToken);
            await AddActorsAsync(row, movie, cancellationToken);

        }

        private static string GetMovieTitle(IXLRow row)
        {
            return row.Cell(1).Value.ToString();
        }

        public static int GetMovieYear(IXLRow row)
        {
            //var 
            return row.Cell(2).GetValue<int>();
        }

        public static string GetMovieDescription(IXLRow row)
        {
            return row.Cell(3).Value.ToString();
        }

        public static int GetMovieLength(IXLRow row)
        {
            return row.Cell(4).GetValue<int>();
        }

        public static string GetMoviePoster(IXLRow row)
        {
            return row.Cell(5).Value.ToString();
        }

        public async Task AddGenreToMovie(Movie movie, Genre genre, CancellationToken cancellationToken)
        {
            var existingRecord = await _catalogContext.MovieGenres.FirstOrDefaultAsync(g => g.Movie == movie && g.Genre == genre, cancellationToken);

            if (existingRecord == null)
            {
                var movieGenre = new MovieGenre
                {
                    Movie = movie,
                    Genre = genre
                };

                _catalogContext.Add(movieGenre);
            }
            
        }

        public async Task AddDirectorsAsync(IXLRow row, Movie movie, CancellationToken cancellationToken)
        {
            int directorStart = 6;
            int directorEnd = 9;
            for (int i = directorStart; i <= directorEnd; i++)
            {
                //string fullName = row.Cell(i).Value.ToString();
                string fullName = row.Cell(i).GetString().Trim();

                if (string.IsNullOrEmpty(fullName)) continue;

                if (fullName.Length < 2 || fullName.Split(' ').Length < 2) continue;


                string[] nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                string firstName = nameParts.Length > 0 ? nameParts[0] : "";
                string lastName = nameParts.Length > 1 ? nameParts[1] : "";

                var director = await _catalogContext.Directors.FirstOrDefaultAsync(d => d.FirstName == firstName && d.LastName == lastName);

                if (director == null)
                {
                    director = new Director 
                    { 
                        FirstName = firstName, 
                        LastName = lastName, 
                        Nationality = "From Excel" 
                    };

                    _catalogContext.Directors.Add(director);

                    //_catalogContext.DirectedBies.Add(new DirectedBy
                    //{
                    //    Director = director,
                    //    Movie = movie
                    //});
                }

                var alreadyLinked = await _catalogContext.DirectedBies
            .FirstOrDefaultAsync(db => db.Movie == movie && db.Director == director, cancellationToken);

                if (alreadyLinked == null)
                {
                    _catalogContext.DirectedBies.Add(new DirectedBy
                    {
                        Director = director,
                        Movie = movie
                    });
                }
            }
        }

        public async Task AddActorsAsync(IXLRow row, Movie movie, CancellationToken cancellationToken)
        {
            int actorStart = 10;
            for (int i = actorStart; i <= row.CellCount(); i++)
            {
                //string fullName = row.Cell(i).Value.ToString();
                string fullName = row.Cell(i).GetString().Trim();

                if (string.IsNullOrEmpty(fullName)) continue;

                if (fullName.Length < 2 || fullName.Split(' ').Length < 2) continue;

                string[] nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                string firstName = nameParts.Length > 0 ? nameParts[0] : "";
                string lastName = nameParts.Length > 1 ? nameParts[1] : "";

                var actor = await _catalogContext.Actors.FirstOrDefaultAsync(d => d.FirstName == firstName && d.LastName == lastName);

                if (actor == null)
                {
                    actor = new Actor
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Nationality = " "
                    };

                    _catalogContext.Actors.Add(actor);
                }

                var alreadyLinked = await _catalogContext.MovieCasts
             .FirstOrDefaultAsync(mc => mc.Movie == movie && mc.Actor == actor, cancellationToken);

                if (alreadyLinked == null)
                {
                    _catalogContext.MovieCasts.Add(new MovieCast
                    {
                        Movie = movie,
                        Actor = actor
                    });
                }



            }
        }



    }
}