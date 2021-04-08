using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePortal.Context;
using MoviePortal.Models.Account;
using MoviePortal.Models.Movy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoviePortal.Controllers
{
    public class MovieController : Controller
    {
        private readonly MoviePortalContext _moviePortalContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieController(MoviePortalContext moviePortalContext, IWebHostEnvironment webHostEnvironment)
        {
            _moviePortalContext = moviePortalContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Movie with this id not found");
            }

            var movie = await _moviePortalContext.Movies.FirstOrDefaultAsync(p => p.Id.Equals(Guid.Parse(id)));

            if (movie == null)
            {
                throw new Exception("Movie with this id not found");
            }

            var movieDTO = new MovieDTO
            {
                CategoryId = movie.CategoryId,
                CategoryName = movie.MovieCategory.Name,
                Description = movie.Description,
                Director = movie.Director,
                Id = movie.Id,
                ImageName = movie.Image,
                InsertDateTime = movie.InsertDateTime,
                InsertUserId = movie.InsertUserId,
                ReleaseDate = movie.ReleaseDate,
                Title = movie.Title,
                UpdateDate = movie.UpdateDate,
                UserName = movie.User.UserName,
            };

            return View(movieDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("GetMovies");
            }

            if (System.IO.File.Exists(""))
            {
                System.IO.File.Delete("");
            }


            var movie = await _moviePortalContext.Movies.FirstOrDefaultAsync(p => p.Id.Equals(Guid.Parse(id)));

            if (movie == null)
            {
                return RedirectToAction("GetMovies");
            }

            var movieDto = new MovieDTO
            {
                CategoryId = movie.CategoryId,
                CategoryName = movie.MovieCategory.Name,
                Description = movie.Description,
                Director = movie.Director,
                Id = movie.Id,
                ImageName = movie.Image,
                InsertDateTime = movie.InsertDateTime,
                InsertUserId = movie.InsertUserId,
                ReleaseDate = movie.ReleaseDate,
                Title = movie.Title,
                UpdateDate = movie.UpdateDate,
                UserName = movie.User.UserName,
                Categories = await _moviePortalContext.MovieCategories.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() }).ToListAsync()
            };
            return View(movieDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var movieDTO = new MovieDTO();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var categories = await _moviePortalContext.MovieCategories.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToListAsync();

            movieDTO.Categories = categories;

            return View(movieDTO);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(MovieDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string finalFileName = null;

            if (model.ImageFile != null)
            {
                var rootPath = _webHostEnvironment.WebRootPath;
                var filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                var fileExtension = Path.GetExtension(model.ImageFile.FileName);
                finalFileName = $"{filename}_{DateTime.Now.ToString("yyMMddHHmmssff")}{fileExtension}";
                var filePath = Path.Combine(rootPath, "images", finalFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }
            }

            if (model.Id != null)
            {

            }

            //Current user id
            var currenUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var movie = new Movie
            {
                CategoryId = model.CategoryId,
                Description = model.Description,
                Director = model.Director,
                Id = Guid.NewGuid(),
                Image = finalFileName,
                InsertDateTime = DateTime.Now,
                InsertUserId = currenUserId,
                IsDelete = false,
                ReleaseDate = model.ReleaseDate,
                Title = model.Title,
            };

            _moviePortalContext.Movies.Add(movie);
            await _moviePortalContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Download(string id)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _moviePortalContext.Movies.Select(m => new MovieDTO
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
    }
}
