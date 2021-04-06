using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviePortal.Models.Movy
{
    public class MovieDTO
    {
        public Guid Id { get; set; }
        [Display(Name = "Title")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }
        [Display(Name = "Director")]
        public string Director { get; set; }
        [Display(Name = "Release date")]
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Display(Name = "Category")]
        [Required]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public string InsertUserId { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string ImageName { get; set; }
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
