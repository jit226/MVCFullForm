using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCFullForm.Models
{
    public class State
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public static List<State> GetStates(int countryId)
        {
            List<State> li = new List<State>();
            li.Add(new State { Name = "Raj", Id = 1, CountryId = 1 });
            li.Add(new State { Name = "Guj", Id = 2, CountryId = 1 });
            li.Add(new State { Name = "Chi", Id = 3, CountryId = 2 });
            li.Add(new State { Name = "Cha", Id = 4, CountryId = 2 });

            return li.Where(c => c.CountryId == countryId).ToList();
        }
    }

}