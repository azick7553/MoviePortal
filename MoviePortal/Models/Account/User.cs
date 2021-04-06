using Microsoft.AspNetCore.Identity;
using MoviePortal.Models.Movy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviePortal.Models.Account
{
    public class User :IdentityUser
    {

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
