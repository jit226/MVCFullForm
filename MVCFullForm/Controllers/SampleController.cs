using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCFullForm.Models;
using System.IO;

namespace MVCFullForm.Controllers
{
    public class SampleController : Controller
    {
        private MVCFullFormDataContext db = new MVCFullFormDataContext();

        // GET: Sample
        public ActionResult Index()
        {
            return View(db.Samples.ToList());
        }

        // GET: Sample/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sample sample = db.Samples.Find(id);
            if (sample == null)
            {
                return HttpNotFound();
            }
            return View(sample);
        }

        // GET: Sample/Create
        public ActionResult Create()
        {
            Sample sample = new Sample();
            sample.HobbiesList = new List<Hobbies>
                {
                    new Hobbies {ID=1,HobbiesName="Playing",IsSelected=false},
                    new Hobbies {ID=2,HobbiesName="Listening Music",IsSelected=false},
                    new Hobbies {ID=3,HobbiesName="Reading",IsSelected=false}

                };
            sample.GenderList = new List<SelectListItem> {
                new SelectListItem { Value = "0", Text = "Male", Selected = false },
                new SelectListItem { Value = "1", Text = "Female", Selected = false },
                new SelectListItem { Value = "2", Text = "Transgender", Selected = false }
                };

            GetCountry();

            return View(sample);
        }

        // POST: Sample/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Gender,CountryId,StateId,CityId,HobbiesList")] Sample sample, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                if (file != null)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                    sample.Image = file.FileName;
                }

                if (sample.HobbiesList.Count() > 0)
                {
                    string hobbiesId = string.Empty;
                    var hobbiesList = sample.HobbiesList.Where(h => h.IsSelected == true).ToList();
                    foreach (var h in hobbiesList)
                    {
                        hobbiesId = hobbiesId + h.ID + ",";
                    }
                    if (hobbiesId.Length > 1)
                    {
                        hobbiesId = hobbiesId.Substring(0, hobbiesId.Length - 1);
                    }
                    sample.Hobbies = hobbiesId;
                }

                sample.DateOfCreation = DateTime.Now;
                sample.LastUpdateOn = DateTime.Now;
                db.Samples.Add(sample);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sample);
        }

        // GET: Sample/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sample sample = db.Samples.Find(id);
            if (sample == null)
            {
                return HttpNotFound();
            }

            string[] hobbiesId = sample.Hobbies.Split(',');

            sample.HobbiesList = new List<Hobbies>
                {
                    new Hobbies {ID=1,HobbiesName="Playing",IsSelected=true},
                    new Hobbies {ID=2,HobbiesName="Listening Music",IsSelected=false},
                    new Hobbies {ID=3,HobbiesName="Reading",IsSelected=false}
                };

            foreach (var h in hobbiesId)
            {
                if (h != "")
                {
                    foreach (var hl in sample.HobbiesList)
                    {
                        if (hl.ID == Convert.ToInt32(h))
                        {
                            hl.IsSelected = true;
                        }
                    }
                }
            }

            sample.GenderList = new List<SelectListItem> {
                new SelectListItem { Value = "0", Text = "Male", Selected = false },
                new SelectListItem { Value = "1", Text = "Female", Selected = false },
                new SelectListItem { Value = "2", Text = "Transgender", Selected = false }
                };

            foreach (var g in sample.GenderList)
            {
                if (g.Value == Convert.ToString(sample.Gender))
                {
                    g.Selected = true;
                }
            }

            GetCountry();
            return View(sample);
        }

        // POST: Sample/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Gender,CountryId,StateId,CityId,HobbiesList")] Sample sample, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                sample.LastUpdateOn = DateTime.Now;

                if (file != null)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                    sample.Image = file.FileName;
                }

                if (sample.HobbiesList.Count() > 0)
                {
                    string hobbiesId = string.Empty;
                    var hobbiesList = sample.HobbiesList.Where(h => h.IsSelected == true).ToList();
                    foreach (var h in hobbiesList)
                    {
                        hobbiesId = hobbiesId + h.ID + ",";
                    }
                    if (hobbiesId.Length > 1)
                    {
                        hobbiesId = hobbiesId.Substring(0, hobbiesId.Length - 1);
                    }
                    sample.Hobbies = hobbiesId;
                }


                db.Entry(sample).State = EntityState.Modified;
                if (file == null)
                {
                    db.Entry(sample).Property(x => x.Image).IsModified = false;
                }
                db.Entry(sample).Property(x => x.DateOfCreation).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sample);
        }

        // GET: Sample/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sample sample = db.Samples.Find(id);
            if (sample == null)
            {
                return HttpNotFound();
            }
            return View(sample);
        }

        // POST: Sample/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sample sample = db.Samples.Find(id);
            db.Samples.Remove(sample);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void GetCountry()
        {
            List<SelectListItem> countries = new List<SelectListItem>();

            Country.GetCountries().ForEach(c =>
            {
                countries.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            });
            countries.Insert(0, new SelectListItem { Text = "Please select Country", Value = "0" });
            ViewData["country"] = countries;
        }

        public JsonResult GetStates(string id)
        {
            List<SelectListItem> states = new List<SelectListItem>();
            State.GetStates(Convert.ToInt32(id)).ForEach(s =>
            {
                states.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString() });
            });
            states.Insert(0, new SelectListItem { Text = "Please select State", Value = "0" });
            return Json(new SelectList(states, "Value", "Text"));
        }

        public JsonResult GetCity(string id)
        {
            List<SelectListItem> cities = new List<SelectListItem>();

            City.GetCities(Convert.ToInt32(id)).ForEach(ct =>
            {
                cities.Add(new SelectListItem { Text = ct.Name, Value = ct.Id.ToString() });
            });
            cities.Insert(0, new SelectListItem { Text = "Please select City", Value = "0" });
            return Json(new SelectList(cities, "Value", "Text"));
        }
    }
}
