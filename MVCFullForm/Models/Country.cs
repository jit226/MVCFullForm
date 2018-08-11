using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCFullForm.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<Country> GetCountries()
        {
            List<Country> li = new List<Country>();
            li.Add(new Country { Name = "India", Id = 1 });
            li.Add(new Country { Name = "China", Id = 2 });
            return li;

        }
    }
}