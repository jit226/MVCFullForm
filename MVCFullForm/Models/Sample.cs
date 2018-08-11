using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCFullForm.Models
{
    public class Sample
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Hobbies { get; set; }
        public string Image { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime LastUpdateOn { get; set; }

        [NotMapped]
        public List<Hobbies> HobbiesList { get; set; }
        [NotMapped]
        public List<SelectListItem> GenderList { get; set; }
    }

}