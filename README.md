###### City.cs

```
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
	
```


###### State.cs

```
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
	
```


###### Country.cs

```
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

```


###### Hobbies.cs

```
    public class Hobbies
    {
        public int ID { get; set; }
        public string HobbiesName { get; set; }
        public bool IsSelected { get; set; }
    }

```

###### Sample.cs

```
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
```


###### MVCFullFormDataContext.cs

```
    public class MVCFullFormDataContext:DbContext
    {
        public DbSet<Sample> Samples { get; set; }
    }
```


###### SampleController.cs

```
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

```

===============================================================
###### Create.cshtml

```
@model MVCFullForm.Models.Sample

@{
    ViewBag.Title = "Create";
}

<h2>Create</h2>


@using (Html.BeginForm("Create", "Sample", FormMethod.Post, new { enctype = "multipart/form-data" }))

{
    @Html.AntiForgeryToken()

    <div class="form-horizontal">
        <h4>Sample</h4>
        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(model => model.Name, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.Name, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.Name, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.Gender, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                <table>
                    @foreach (var g in Model.GenderList)
                    {
                        <tr>
                            <td>
                                @Html.RadioButtonFor(model=> model.Gender, g.Value, g.Selected)
                            </td>
                            <td>
                                @Html.Label(g.Text, new { @for = g.Value })
                            </td>
                        </tr>
                    }
                </table>
                @Html.ValidationMessageFor(model => model.Gender, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.CountryId, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.DropDownListFor(model => model.CountryId, ViewData["country"] as List<SelectListItem>, new { style = "width:250px", @class = "form-control" })
                @Html.ValidationMessageFor(model => model.CountryId, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.StateId, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.DropDownListFor(model => model.StateId, new SelectList(string.Empty, "Value", "Text"), new { style = "width:250px", @class = "form-control" })
                @Html.ValidationMessageFor(model => model.StateId, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.CityId, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.DropDownListFor(model => model.CityId, new SelectList(string.Empty, "Value", "Text"), new { style = "width:250px", @class = "form-control" })
                @Html.ValidationMessageFor(model => model.CityId, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.Image, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.TextBox("file", "", new { type = "file" })
            </div>
        </div>
        <div class="form-group">
            <table>
                @for (int idx = 0; idx < Model.HobbiesList.Count; idx++)
                {
                    <tr>
                        <td>
                            @Html.HiddenFor(x => Model.HobbiesList[idx].ID)
                            @Html.CheckBoxFor(x => Model.HobbiesList[idx].IsSelected)
                            @Html.DisplayFor(x => Model.HobbiesList[idx].HobbiesName)
                            @Html.HiddenFor(x => Model.HobbiesList[idx].HobbiesName)
                        </td>
                    </tr>
                }
            </table>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Create" class="btn btn-default" />
            </div>
        </div>
    </div>
}

<div>
    @Html.ActionLink("Back to List", "Index")
</div>

@section Scripts {
    @Scripts.Render("~/bundles/jqueryval")

    <script type="text/javascript">

        $(document).ready(function () {
            $("#CountryId").change(function () {
                var countryId = $("#CountryId").val();
                getState(countryId);
            })

            $("#StateId").change(function () {
                var stateId = $("#StateId").val();
                getCity(stateId);
            })

            function getState(cid) {
                $("#StateId").empty();
                $.ajax({
                    type: 'POST',
                    url: '@Url.Action("GetStates")',
                    dataType: 'json',
                    data: { id: cid },
                    success: function (states) {
                        $.each(states, function (i, state) {
                            $("#StateId").append('<option value="' + state.Value + '">' +
                                state.Text + '</option>');
                        });
                    },
                    error: function (ex) {
                        alert('Failed to retrieve states.' + ex);
                    }
                });
                return false;
            }

            function getCity(sId) {
                $("#CityId").empty();
                $.ajax({
                    type: 'POST',
                    url: '@Url.Action("GetCity")',
                    dataType: 'json',
                    data: { id: sId },
                    success: function (citys) {
                        $.each(citys, function (i, city) {
                            $("#CityId").append('<option value="' + city.Value + '">' + city.Text + '</option>');
                        });
                    },
                    error: function (ex) {
                        alert('Failed to retrieve states.' + ex);
                    }
                });
                return false;
            }
        });

    </script>
}

```


###### Edit.cshtml

```

@model MVCFullForm.Models.Sample

@{
    /**/

    ViewBag.Title = "Edit";
}

<h2>Edit</h2>

@{
    var tempStateId = Model.StateId;
    var tempCityId = Model.CityId;
}
@using (Html.BeginForm("Edit", "Sample", FormMethod.Post, new { enctype = "multipart/form-data" }))
{
    @Html.AntiForgeryToken()

    <div class="form-horizontal">
        <h4>Sample</h4>
        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        @Html.HiddenFor(model => model.Id)

        <div class="form-group">
            @Html.LabelFor(model => model.Name, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.Name, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.Name, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.Gender, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                <table>
                    @foreach (var g in Model.GenderList)
                    {
                        <tr>
                            <td>
                                @Html.RadioButtonFor(model => model.Gender, g.Value, g.Selected)
                            </td>
                            <td>
                                @Html.Label(g.Text, new { @for = g.Value })
                            </td>
                        </tr>
                    }
                </table>
                @Html.ValidationMessageFor(model => model.Gender, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.CountryId, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.DropDownListFor(model => model.CountryId, ViewData["country"] as List<SelectListItem>, new { style = "width:250px", @class = "form-control" })
                @Html.ValidationMessageFor(model => model.CountryId, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.StateId, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.DropDownListFor(model => model.StateId, new SelectList(string.Empty, "Value", "Text"), new { style = "width:250px", @class = "form-control" })
                @Html.ValidationMessageFor(model => model.StateId, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.CityId, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.DropDownListFor(model => model.CityId, new SelectList(string.Empty, "Value", "Text"), new { style = "width:250px", @class = "form-control" })
                @Html.ValidationMessageFor(model => model.CityId, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.Image, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.TextBox("file", "", new { type = "file" })
            </div>
        </div>

        <div class="form-group">
            <table>
                @for (int idx = 0; idx < Model.HobbiesList.Count; idx++)
                {
                    <tr>
                        <td>
                            @Html.HiddenFor(x => Model.HobbiesList[idx].ID)
                            @Html.CheckBoxFor(x => Model.HobbiesList[idx].IsSelected)
                            @Html.DisplayFor(x => Model.HobbiesList[idx].HobbiesName)
                            @Html.HiddenFor(x => Model.HobbiesList[idx].HobbiesName)
                        </td>
                    </tr>
                }
            </table>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Save" class="btn btn-default" />
            </div>
        </div>
    </div>
}

<div>
    @Html.ActionLink("Back to List", "Index")
</div>

@section Scripts {
    @Scripts.Render("~/bundles/jqueryval")


    <script type="text/javascript">

        $(document).ready(function () {

            var countryId = $("#CountryId").val();

            if (countryId != "" && countryId != "0") {
                getState(countryId);
                var sID = '@tempStateId';
                if (sID != "" && sID != "0") {
                    $("#StateId option[value='" + sID + "']").attr("selected", "selected");
                    var stateId = $("#StateId").val();
                    var cID = '@tempCityId';
                    if (stateId != "" && stateId != "0") {
                        getCity(stateId);
                        $("#CityId option[value='" + cID + "']").attr("selected", "selected");
                    }
                }
            }

            $("#CountryId").change(function () {
                var countryId = $("#CountryId").val();
                getState(countryId);
            })

            $("#StateId").change(function () {
                var stateId = $("#StateId").val();
                getCity(stateId);
            })

            function getState(cid) {

                $("#StateId").empty();
                $.ajax({
                    type: 'POST',
                    url: '@Url.Action("GetStates")',
                    dataType: 'json',
                    data: { id: cid },
                    async: false,
                    success: function (states) {
                        $.each(states, function (i, state) {
                            $("#StateId").append('<option value="' + state.Value + '">' +
                                state.Text + '</option>');
                        });
                    },
                    error: function (ex) {
                        alert('Failed to retrieve states.' + ex);
                    }
                });
                return false;
            }

            function getCity(sId) {
                $("#CityId").empty();
                $.ajax({
                    type: 'POST',
                    url: '@Url.Action("GetCity")',
                    dataType: 'json',
                    data: { id: sId },
                    async: false,
                    success: function (citys) {
                        $.each(citys, function (i, city) {
                            $("#CityId").append('<option value="' + city.Value + '">' + city.Text + '</option>');
                        });
                    },
                    error: function (ex) {
                        alert('Failed to retrieve states.' + ex);
                    }
                });
                return false;
            }
        });

    </script>
}

```

###### Index.cshtml

```
@model IEnumerable<MVCFullForm.Models.Sample>

@{
    ViewBag.Title = "Index";
}

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(model => model.Name)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.Gender)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.CountryId)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.StateId)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.CityId)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.Hobbies)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.Image)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.DateOfCreation)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.LastUpdateOn)
        </th>
        <th></th>
    </tr>

    @foreach (var item in Model)
    {
        <tr>
            <td>
                @Html.DisplayFor(modelItem => item.Name)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.Gender)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.CountryId)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.StateId)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.CityId)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.Hobbies)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.Image)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.DateOfCreation)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.LastUpdateOn)
            </td>
            <td>
                @Html.ActionLink("Edit", "Edit", new { id = item.Id }) |
                @Html.ActionLink("Details", "Details", new { id = item.Id }) |
                @Html.ActionLink("Delete", "Delete", new { id = item.Id })
            </td>
        </tr>
    }

</table>

```


###### Delete.cshtml

```
@model MVCFullForm.Models.Sample

@{
    ViewBag.Title = "Delete";
}

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>Sample</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(model => model.Name)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.Name)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.Gender)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.Gender)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.CountryId)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.CountryId)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.StateId)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.StateId)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.CityId)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.CityId)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.Hobbies)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.Hobbies)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.Image)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.Image)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.DateOfCreation)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.DateOfCreation)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.LastUpdateOn)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.LastUpdateOn)
        </dd>

    </dl>

    @using (Html.BeginForm())
    {
        @Html.AntiForgeryToken()

        <div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    }
</div>

```


###### Details.cshtml

```
@model MVCFullForm.Models.Sample

@{
    ViewBag.Title = "Details";
}

<h2>Details</h2>

<div>
    <h4>Sample</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(model => model.Name)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.Name)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.Gender)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.Gender)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.CountryId)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.CountryId)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.StateId)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.StateId)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.CityId)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.CityId)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.Hobbies)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.Hobbies)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.Image)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.Image)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.DateOfCreation)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.DateOfCreation)
        </dd>

        <dt>
            @Html.DisplayNameFor(model => model.LastUpdateOn)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.LastUpdateOn)
        </dd>

    </dl>
</div>
<p>
    @Html.ActionLink("Edit", "Edit", new { id = Model.Id }) |
    @Html.ActionLink("Back to List", "Index")
</p>
```
