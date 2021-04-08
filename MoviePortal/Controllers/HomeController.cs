using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviePortal.Context;
using MoviePortal.Models;
using MoviePortal.Models.Movy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MoviePortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MoviePortalContext _moviePortalContext;

        public HomeController(ILogger<HomeController> logger, MoviePortalContext moviePortalContext)
        {
            _logger = logger;
            _moviePortalContext = moviePortalContext;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _moviePortalContext.Movies.Select(m=> new MovieDTO 
            {
                CategoryId = m.CategoryId,
                CategoryName = m.MovieCategory.Name,
                Description = m.Description,
                Director = m.Director,
                Id = m.Id,
                ImageName = m.Image,
                InsertDateTime = m.InsertDateTime,
                InsertUserId = m.InsertUserId,
                ReleaseDate = m.ReleaseDate,
                Title = m.Title,
                UpdateDate = m.UpdateDate,
                UserId = m.User.Id,
                UserName = m.User.UserName
            }).ToListAsync();
            return View(movies);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
