using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviePortal.Context;
using MoviePortal.Models.Movy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviePortal.Services.Movie
{
    public class MovieService
    {
        private readonly MoviePortalContext _context;
        private readonly IMapper _mapper;

        public MovieService(MoviePortalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MovieDTO>> GetAll()
        {
            var movies = await _context.Movies.Select(m => new MovieDTO
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

            return movies;
        }

        public async Task<MovieDTO> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Movie with this id not found");
            }

            var movie = await _context.Movies.FirstOrDefaultAsync(p => p.Id.Equals(Guid.Parse(id)));

            if (movie == null)
            {
                throw new Exception("Movie with this id not found");
            }

            var movieDTO = _mapper.Map<MovieDTO>(movie);

            movieDTO.Categories = await _context.MovieCategories.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() }).ToListAsync();

            return movieDTO;
        }

        public async Task Update(MovieDTO model, string fileName)
        {
            //var movie = await _context.Movies.FirstOrDefaultAsync(p => p.Id.Equals(model.Id));

            //if (movie == null)
            //{
            //    throw new Exception($"Movie with id: {model.Id} not found");
            //}

            var movie = _mapper.Map<MoviePortal.Models.Movy.Movie>(model);

            movie.Image = string.IsNullOrEmpty(fileName) ? movie.Image : fileName;
          
            movie.UpdateDate = DateTime.Now;

            _context.Movies.Update(movie);

            await _context.SaveChangesAsync();
        }
    }
}
