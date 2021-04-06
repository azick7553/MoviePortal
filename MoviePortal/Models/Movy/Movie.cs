using MoviePortal.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MoviePortal.Models.Movy
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Guid CategoryId { get; set; }
        public string InsertUserId { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string Image { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }

        [ForeignKey("InsertUserId")]
        public virtual User User { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual MovieCategory MovieCategory { get; set; }
    }
}
