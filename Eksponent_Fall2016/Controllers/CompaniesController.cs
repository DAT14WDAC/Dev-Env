using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eksponent_Fall2016.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using System.Web.Configuration;

namespace Eksponent_Fall2016.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Companies
        public ActionResult Index()
        {
            //Fetching UserManager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Get User from Database based on userId 
            var currentUser = userManager.FindById(User.Identity.GetUserId());

            Company company = db.Companies.Where(x => x.ApplicationUserId == currentUser.Id).SingleOrDefault();

            return View(company);
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.skills = new SelectList(db.Skills, "CompanyId", "Skillname");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return RedirectToAction("Create", "Skills");
            //return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyId,CompanyName,CompanyDescription,CompanyLogo,ApplicationUserId")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyId,CompanyName,CompanyDescription,CompanyLogo,ApplicationUserId")] Company company,
            HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var path = Server != null ? Server.MapPath("~") : "";
                company.SaveLogo(image, path, "/ProfileImages/");
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            //Fetching UserManager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Get User from Database based on userId 
            var currentUser = userManager.FindById(User.Identity.GetUserId());

            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.Users.Remove(currentUser);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // GET: Companies/get sills list
        public ActionResult GetSkills()
        {

            //Fetching UserManager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Get User from Database based on userId 
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            //get cuurent company from db
            Company company = db.Companies.Where(x => x.ApplicationUserId == currentUser.Id).Single();

            var list = new List<Skill>();
            list = db.Skills.Where(x => x.CompanyId == company.CompanyId).ToList();

            var model = new EmployeeSkillViewModel
            {
                SkillList = list.Select(a => new SelectListItem
                {
                    Text = a.Skillname,
                    Value = a.SkillId.ToString(),
                })
            };

            return View(model);
        }

        // POST: Companies/fetch list skills
        [HttpPost]
        public ActionResult GetEmployeeSkills(IEnumerable<int> skillIds)
        {
            //Fetching UserManager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Get User from Database based on userId 
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            //get the current company from db
            Company company = db.Companies.Where(x => x.ApplicationUserId == currentUser.Id).Single();

            var eSList = new List<EmployeeSkill>();

            if (ModelState.IsValid)
            {
                foreach (var skill in skillIds)
                {
                    var result = db.EmployeesSkills.Include(e => e.Employee).Include(e => e.Skill)
                   .Where(x => x.SkillId == skill).ToList();
                    eSList.AddRange(result);
                }
            }
            return View(eSList);
        }

        // GET: Companies/Experience Overview
        [HttpGet]
        public ActionResult ExperienceOverview()
        {
            var model = new EmployeeSkillViewModel
            {
                LevelList = new List<SelectListItem>()
                {
                        new SelectListItem{ Text="1", Value="1"},
                        new SelectListItem{ Text="2", Value="2"},
                        new SelectListItem{ Text="3", Value="3"},
                        new SelectListItem{ Text="4", Value="4"},
                        new SelectListItem{ Text="5", Value="5"}
                }
            };
            return View(model);
        }

        // Post: Companies/Experience Overview 
        [HttpPost, ActionName("ExperienceOverview")]
        public ActionResult ExperienceOverview(int radioIds)
        {
            // get the employees with level and count
            var employee = db.EmployeesSkills.Include(e => e.Employee).Where(l => l.Level == radioIds).Count();

            if (ModelState.IsValid && employee != 0)
            {
                var model = new EmployeeSkillViewModel
                {
                    Level = employee
                };
                return View(model);
            }
            return View(ViewBag.Message = "No level experience found within your company.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
   

}
