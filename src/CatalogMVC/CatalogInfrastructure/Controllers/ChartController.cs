using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private record CountMoviesByYear(string year, int count);
        private record TopRatedMovies(string title, int ratingValue);

        private readonly DbCatalogContext catalogContext;

        public ChartController(DbCatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }

        [HttpGet("countByYear")]
        public async Task<JsonResult> GetCountMoviesByYearAsync(CancellationToken cancellationToken)
        {
            var responseItems = await catalogContext
            .Movies
            .GroupBy(movie => movie.Year)
            .Select(group => new CountMoviesByYear(group.Key.ToString(), group.Count()))
            .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }

        [HttpGet("sources")]
        public async Task<JsonResult> GetAvailableSourcesAsync(CancellationToken cancellationToken)
        {
            var sources = await catalogContext
                .Ratings
                .Select(r => r.Source)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync(cancellationToken);

            return new JsonResult(sources);
        }


        [HttpGet("topRatedMovies")]
        public async Task<JsonResult> GetTopRatedMoviesBySourceAsync([FromQuery] string source, CancellationToken cancellationToken)
        {
            var responseItems = await catalogContext
                .Movies
                .Where(m => m.Ratings.Any(r => r.Source == source))
                .Select(m => new
                {
                    m.Title,
                    ratingValue = m.Ratings
                        .Where(r => r.Source == source)
                        .Average(r => r.Value)
                })
                .OrderByDescending(m => m.ratingValue)
                .Take(10)
                .ToListAsync(cancellationToken);


            return new JsonResult(responseItems);
        }




    }
}