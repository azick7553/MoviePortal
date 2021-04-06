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

            var movie = await _moviePortalContext.Movies.FirstOrDefaultAsync(p=> p.Id.Equals(Guid.Parse(id)));

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

        [HttpGet]
        [Authorize]
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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create (MovieDTO model)
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
    }
}
