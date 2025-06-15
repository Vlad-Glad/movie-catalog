using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System;
using CatalogDomain.Model;

namespace CatalogInfrastructure.Controllers
{
    public class QueriesController : Controller
    {
        private readonly DbCatalogContext _context;

        public QueriesController(DbCatalogContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult SearchByGenre()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchByGenre(string genreName)
        {
            var result = await _context.Movies
                .FromSqlRaw(@"
                SELECT m.* 
                FROM Movie m
                JOIN MovieGenre mg ON m.Id = mg.MovieID
                JOIN Genre g ON g.Id = mg.GenreID
                WHERE g.GenreName = {0}", genreName)
                .ToListAsync();

            return View("~/Views/Home/Index.cshtml", result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchByDirector(string directorLastName)
        {
            var result = await _context.Movies
                .FromSqlRaw(@"
                SELECT m.*
                FROM Movie m
                JOIN DirectedBy db ON m.Id = db.MovieId
                JOIN Director d ON d.Id = db.DirectorId
                WHERE d.LastName = {0}", directorLastName)
                .ToListAsync();

            return View("~/Views/Home/Index.cshtml", result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchByActor(string actorLastName)
        {

            var result = await _context.Movies
                .FromSqlRaw(@"
                SELECT DISTINCT m.*
                FROM Movie m
                JOIN MovieCast mc ON m.Id = mc.MovieId
                JOIN Actor a ON a.Id = mc.ActorId
                WHERE a.LastName = {0}", actorLastName)
                .ToListAsync();

            return View("~/Views/Home/Index.cshtml", result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchActorsByMinMovies(int minCount)
        {
            if (minCount <= 0)
            {
                ViewBag.AllGenreDirectors = null;
                return View("~/Views/Home/Index.cshtml");
            }

            var result = await _context.Actors
                .FromSqlRaw(@"
                SELECT a.*
                FROM Actor a
                JOIN MovieCast mc ON a.Id = mc.ActorId
                GROUP BY a.Id, a.FirstName, a.LastName, a.Nationality
                HAVING COUNT(mc.MovieId) > {0}", minCount)
                .ToListAsync();

            ViewBag.ActorResults = result;
            return View("~/Views/Home/Index.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> SearchDirectorsByMinMovies(int minCount)
        {
            if (minCount <= 0)
            {
                ViewBag.AllGenreDirectors = null;
                return View("~/Views/Home/Index.cshtml");
            }

            var result = await _context.Directors
                .FromSqlRaw(@"
                SELECT d.*
                FROM Director d
                JOIN DirectedBy db ON d.Id = db.DirectorId
                GROUP BY d.Id, d.FirstName, d.LastName, d.Nationality
                HAVING COUNT(db.MovieId) > {0}", minCount)
                .ToListAsync();

            ViewBag.DirectorResults = result;
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ActorsWithSameMovies(string actorLastName)
        {
            if (string.IsNullOrWhiteSpace(actorLastName))
            {
                ViewBag.SameMovieActors = null;
                return View("~/Views/Home/Index.cshtml");
            }

            var result = await _context.Actors
                .FromSqlRaw(@"
            SELECT a2.*
            FROM Actor a2
            WHERE a2.Id != (
                SELECT a1.Id
                FROM Actor a1
                WHERE a1.LastName = {0}
            )
            AND NOT EXISTS (
                SELECT *
                FROM MovieCast mc1
                WHERE mc1.ActorId = (
                    SELECT a1.Id FROM Actor a1 WHERE a1.LastName = {0}
                )
                AND NOT EXISTS (
                    SELECT *
                    FROM MovieCast mc2
                    WHERE mc2.ActorId = a2.Id AND mc2.MovieId = mc1.MovieId
                )
            )
            AND NOT EXISTS (
                SELECT *
                FROM MovieCast mc2
                WHERE mc2.ActorId = a2.Id
                AND NOT EXISTS (
                    SELECT *
                    FROM MovieCast mc1
                    WHERE mc1.ActorId = (
                        SELECT a1.Id FROM Actor a1 WHERE a1.LastName = {0}
                    )
                    AND mc1.MovieId = mc2.MovieId
                )
            )", actorLastName)
                .ToListAsync();

            ViewBag.SameMovieActors = result;
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> DirectorsOfAllMoviesByGenre(string genreName)
        {
            if (string.IsNullOrWhiteSpace(genreName))
            {
                ViewBag.AllGenreDirectors = null;
                return View("~/Views/Home/Index.cshtml");
            }

            var result = await _context.Directors
                .FromSqlRaw(@"
            SELECT d.*
            FROM Director d
            WHERE NOT EXISTS (
                SELECT m.Id
                FROM Movie m
                JOIN MovieGenre mg ON m.Id = mg.MovieId
                JOIN Genre g ON g.Id = mg.GenreId
                WHERE g.GenreName = {0}
                AND NOT EXISTS (
                    SELECT *
                    FROM DirectedBy db
                    WHERE db.DirectorId = d.Id AND db.MovieId = m.Id
                )
            )", genreName)
                .ToListAsync();

            ViewBag.AllGenreDirectors = result;
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ActorsWithIdenticalFilmography()
        {
            var result = await _context.ActorPairs
                .FromSqlRaw(@"
            SELECT 
                a1.Id AS Actor1Id, a2.Id AS Actor2Id, 
                a1.FirstName AS Actor1FirstName, a1.LastName AS Actor1LastName,
                a2.FirstName AS Actor2FirstName, a2.LastName AS Actor2LastName,
                STRING_AGG(m.Title, ', ') AS Movies
            FROM Actor a1
            JOIN Actor a2 ON a1.Id < a2.Id
            JOIN MovieCast mc1 ON a1.Id = mc1.ActorId
            JOIN MovieCast mc2 ON a2.Id = mc2.ActorId AND mc1.MovieId = mc2.MovieId
            JOIN Movie m ON m.Id = mc1.MovieId
            WHERE NOT EXISTS (
                SELECT *
                FROM MovieCast mca
                WHERE mca.ActorId = a1.Id
                AND NOT EXISTS (
                    SELECT *
                    FROM MovieCast mcb
                    WHERE mcb.ActorId = a2.Id AND mcb.MovieId = mca.MovieId
                )
            )
            AND NOT EXISTS (
                SELECT *
                FROM MovieCast mcb
                WHERE mcb.ActorId = a2.Id
                AND NOT EXISTS (
                    SELECT *
                    FROM MovieCast mca
                    WHERE mca.ActorId = a1.Id AND mca.MovieId = mcb.MovieId
                )
            )
            GROUP BY 
                a1.Id, a2.Id, 
                a1.FirstName, a1.LastName, 
                a2.FirstName, a2.LastName
        ").ToListAsync();

            ViewBag.ActorPairs = result;
            return View("~/Views/Home/Index.cshtml");
        }


    }
}
