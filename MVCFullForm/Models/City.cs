using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCFullForm.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public static List<City> GetCities(int stateId)
        {
            List<City> li = new List<City>();
            li.Add(new City { Name = "Jal", Id = 1, StateId = 1 });
            li.Add(new City { Name = "Pal", Id = 2, StateId = 1 });
            li.Add(new City { Name = "Ahm", Id = 3, StateId = 2 });
            li.Add(new City { Name = "Sur", Id = 4, StateId = 2 });
            return li.Where(ct => ct.StateId == stateId).ToList();
        }
    }
}