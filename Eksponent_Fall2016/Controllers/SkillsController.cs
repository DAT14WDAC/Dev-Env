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

namespace Eksponent_Fall2016.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class SkillsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Skills
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Index()
        {
            //Fetching UserManager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Get User from Database based on userId 
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            Company company = db.Companies.Where(x => x.ApplicationUserId == currentUser.Id).FirstOrDefault();

            var skills = db.Skills.Include(s => s.Company).Where(x => x.CompanyId == company.CompanyId);
            return View(skills.ToList());
        }
     //   [Authorize(Roles = "Employee")]
        public ActionResult EmployeeSkills(int? id)
        {
       
            Company company = db.Companies.Where(x => x.CompanyId == id).FirstOrDefault();
            var skills = db.Skills.Where(x => x.CompanyId == company.CompanyId).ToList();
            ViewBag.Company = company.CompanyName; 
            return View(skills);
        }
        public ActionResult AddSkill()
        {

            return RedirectToAction("Create", "EmployeeSkills");
        }
        public ActionResult FocusSkill()
        {
            return RedirectToAction("Create", "SkillFocus");
        }
        // GET: Skills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
        }

        // GET: Skills/Create
        public ActionResult Create()
        {
            //Fetching UserManager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Get User from Database based on userId 
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            Company company = db.Companies.Where(x => x.ApplicationUserId == currentUser.Id).Single();

            ViewBag.CompanyId = company.CompanyId;

            return View();
        }

        // POST: Skills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SkillId,Skillname,,CompanyId,Skilldescription")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                db.Skills.Add(skill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName", skill.CompanyId);
            return View(skill);
        }

        // GET: Skills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            //Fetching UserManager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Get User from Database based on userId 
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            Company company = db.Companies.Where(x => x.ApplicationUserId == currentUser.Id).Single();

            ViewBag.CompanyId = company.CompanyId;
            return View(skill);
        }

        // POST: Skills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SkillId,Skillname,CompanyId,Skilldescription")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(skill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //Fetching UserManager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Get User from Database based on userId 
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            Company company = db.Companies.Where(x => x.ApplicationUserId == currentUser.Id).Single();

            ViewBag.CompanyId = company.CompanyId;
            return View(skill);
        }

        // GET: Skills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
        }

        // POST: Skills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Skill skill = db.Skills.Find(id);
            db.Skills.Remove(skill);
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
    }
}
