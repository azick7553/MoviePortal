using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePortal.Context;
using MoviePortal.Extensions.DateTimeExtensions;
using MoviePortal.Models.Account;
using MoviePortal.Models.Movy;
using MoviePortal.Services.Movie;
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
        private readonly MovieService _movieService;

        public MovieController(
            MoviePortalContext moviePortalContext, 
            IWebHostEnvironment webHostEnvironment,
            MovieService movieService)
        {
            _moviePortalContext = moviePortalContext;
            _webHostEnvironment = webHostEnvironment;
            _movieService = movieService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var movie = await _movieService.GetById(id);
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var movie = await _movieService.GetById(id);
            return View(movie);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteImage(string imageName, string movieId)
        {
            if (string.IsNullOrEmpty(imageName) || string.IsNullOrEmpty(movieId))
            {
                return BadRequest();
            }

            var result = await DeleteFile(imageName, movieId);

            if (!result)
            {
                return BadRequest();
            }

            return Ok("Success");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(MovieDTO model)
        {
            var fileName = "";
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ImageFile != null)
            {
                await DeleteFile(model.ImageName, model.Id.ToString());
                fileName = await CopyFile(model.ImageFile);
            }

            await _movieService.Update(model, fileName);

            return RedirectToAction("GetMovies");
        }

        [NonAction]
        private async Task<bool> DeleteFile(string imageName, string movieId)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                return false;
            }
            var rootPath = _webHostEnvironment.WebRootPath;
            var finalfilePath = Path.Combine(rootPath, "images", imageName);

            if (!System.IO.File.Exists(finalfilePath))
            {
                return false;
            }

            await Task.Run(() =>
            {
                System.IO.File.Delete(finalfilePath);
            });

            var parsedMovieIdResult = Guid.TryParse(movieId, out var parsedMovieId);

            if (!parsedMovieIdResult)
            {
                return false; ;
            }

            var movie = await _moviePortalContext.Movies.FirstOrDefaultAsync(p => p.Id.Equals(parsedMovieId));

            if (movie == null)
            {
                return false;
            }

            movie.Image = null;

            await _moviePortalContext.SaveChangesAsync();

            return true;
        }

        [Authorize(Roles = "Admin")]
        [NonAction]
        private async Task<string> CopyFile(IFormFile imageFile)
        {
            if (imageFile == null) return null;

            var rootPath = _webHostEnvironment.WebRootPath;
            var filename = Path.GetFileNameWithoutExtension(imageFile.FileName); //02animalpicture
            var fileExtension = Path.GetExtension(imageFile.FileName); //.jpeg
            var finalFileName = $"{filename}_{DateTime.Now.ToString("yyMMddHHmmssff")}{fileExtension}";
            var filePath = Path.Combine(rootPath, "images", finalFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return finalFileName;
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
                finalFileName = await CopyFile(model.ImageFile);
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
            //var dateString = DateTime.Now.ToStringTajikFormat(DateFormats.Russian);

            var movies = await _movieService.GetAll();
            return View(movies);
        }
    }

    //public enum DateFormats
    //{
    //    Tajikistan,
    //    Russian
    //}
}
