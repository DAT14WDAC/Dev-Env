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
    public class SkillFocusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SkillFocus
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            Employee e = db.Employees.Where(i => i.ApplicationUserId == currentUser.Id).FirstOrDefault();
            var skillFocus = db.SkillFocus.Where(i => i.EmployeeId == e.EmployeeId).Include(s => s.Skill);
            return View(skillFocus.ToList());
        }

        // GET: SkillFocus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillFocus skillFocus = db.SkillFocus.Find(id);
            if (skillFocus == null)
            {
                return HttpNotFound();
            }
            return View(skillFocus);
        }

        // GET: SkillFocus/Create
        public ActionResult Create()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            Employee e = db.Employees.Where(i => i.ApplicationUserId == currentUser.Id).FirstOrDefault();
            var list = new List<Skill>();
            list = db.Skills.Where(x => x.CompanyId == e.CompanyId).ToList();
            var list2 = new List<SkillFocus>();
            foreach (var item in e.ISkillFocus)
            {
                list2.Add(item);
            }
            for (int i = 0; i < list.Count(); i++)
            {
                for (int j = 0; j < list2.Count(); j++)
                {
                    if (list[i].SkillId == list2[j].SkillId)
                    {
                        list.RemoveAt(i);
                    }
                }
            }

            var model = new SkillFocusViewModel
            {
                SkillList = list.Select(a => new SelectListItem
                {
                    Text = a.Skillname,
                    Value = a.SkillId.ToString()
                }),
                NumberOfFocus = list.Count()  

            };
            return View(model);
        }

        // POST: SkillFocus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SkillFocusId,EmployeeId,SkillId,Startdate")] SkillFocus skillFocus)
        {
            if (ModelState.IsValid)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                Employee employee = db.Employees.Where(i => i.ApplicationUserId == currentUser.Id).FirstOrDefault();
                skillFocus.EmployeeId = employee.EmployeeId;
                skillFocus.Startdate = DateTime.Now;
                skillFocus.Employee = employee;
                skillFocus.Skill = db.Skills.Find(skillFocus.SkillId);
                employee.ISkillFocus.Add(skillFocus);
                db.SkillFocus.Add(skillFocus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Firstname", skillFocus.EmployeeId);
            ViewBag.SkillId = new SelectList(db.Skills, "SkillId", "Skillname", skillFocus.SkillId);
            return View(skillFocus);
        }

        // GET: SkillFocus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillFocus skillFocus = db.SkillFocus.Find(id);
            if (skillFocus == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Firstname", skillFocus.EmployeeId);
            ViewBag.SkillId = new SelectList(db.Skills, "SkillId", "Skillname", skillFocus.SkillId);
            return View(skillFocus);
        }

        // POST: SkillFocus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SkillFocusId,EmployeeId,SkillId,Startdate")] SkillFocus skillFocus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(skillFocus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Firstname", skillFocus.EmployeeId);
            ViewBag.SkillId = new SelectList(db.Skills, "SkillId", "Skillname", skillFocus.SkillId);
            return View(skillFocus);
        }

        // GET: SkillFocus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillFocus skillFocus = db.SkillFocus.Find(id);
            if (skillFocus == null)
            {
                return HttpNotFound();
            }
            return View(skillFocus);
        }

        // POST: SkillFocus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SkillFocus skillFocus = db.SkillFocus.Find(id);
            db.SkillFocus.Remove(skillFocus);
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
