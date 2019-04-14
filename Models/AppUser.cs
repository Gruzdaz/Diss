using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diss.Models
{
    public class AppUser : IdentityUser
    {
        public int ELO { get; set; }
    }
}