using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCFullForm.Models
{
    public class MVCFullFormDataContext:DbContext
    {
        public DbSet<Sample> Samples { get; set; }
    }
}