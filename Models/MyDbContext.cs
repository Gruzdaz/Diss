using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Diss.Models
{
    public class MyDbContext : IdentityDbContext<AppUser>
    {
        public MyDbContext() : base("MyDbContext")
        {
        }

        public DbSet<Event> Events { get; set; }

    }
}