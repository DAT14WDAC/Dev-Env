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
    public class EmployeeSkillsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EmployeeSkills
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            Employee e = db.Employees.Where(i => i.ApplicationUserId == currentUser.Id).FirstOrDefault();
            var employeesSkills = db.EmployeesSkills.Where(i => i.EmployeeId == e.EmployeeId).Include(i => i.Skill);
            return View(employeesSkills.ToList());
        }

        // GET: EmployeeSkills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSkill employeeSkill = db.EmployeesSkills.Find(id);
            if (employeeSkill == null)
            {
                return HttpNotFound();
            }
            return View(employeeSkill);
        }

        // GET: EmployeeSkills/Create
        public ActionResult Create()
        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            Employee e = db.Employees.Where(i => i.ApplicationUserId == currentUser.Id).FirstOrDefault();
            var list = new List<Skill>();
            list = db.Skills.Where(x => x.CompanyId == e.CompanyId).ToList();
            var list2 = new List<EmployeeSkill>();
            foreach (var item in e.IEmployeeSkill)
            {
                list2.Add(item);
            }
            for(int i = 0; i < list.Count(); i++)
            {
                for (int j = 0; j < list2.Count(); j++)
                {
              if (list[i].SkillId == list2[j].SkillId)
                    {
                        list.RemoveAt(i);
                    }
                }
            }
            var model = new EmployeeSkillViewModel
            {
                SkillList = list.Select(a => new SelectListItem
                {
                    Text = a.Skillname,
                    Value = a.SkillId.ToString()
                })
              
            };
           
            return View(model);
        }

        // POST: EmployeeSkills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeSkillId,Level,SkillId,EmployeeId")] EmployeeSkillViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                Employee employee = db.Employees.Where(i => i.ApplicationUserId == currentUser.Id).FirstOrDefault();
                EmployeeSkill e = new EmployeeSkill();
                e.EmployeeId = employee.EmployeeId;
                e.Level = model.Level;
                e.SkillId = model.SkillId;
                employee.IEmployeeSkill.Add(e);
                db.EmployeesSkills.Add(e);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: EmployeeSkills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSkill employeeSkill = db.EmployeesSkills.Find(id);
            if (employeeSkill == null)
            {
                return HttpNotFound();
            }
          
            return View(employeeSkill);
        }

        // POST: EmployeeSkills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeSkillId,Level,SkillId,EmployeeId")] EmployeeSkill employeeSkill)
        {
            if (ModelState.IsValid)
            {
                EmployeeSkill e = db.EmployeesSkills.Find(employeeSkill.EmployeeSkillId);
                e.Level = employeeSkill.Level;
            
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(employeeSkill);
        }

        // GET: EmployeeSkills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSkill employeeSkill = db.EmployeesSkills.Find(id);
            if (employeeSkill == null)
            {
                return HttpNotFound();
            }
            return View(employeeSkill);
        }

        // POST: EmployeeSkills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeSkill employeeSkill = db.EmployeesSkills.Find(id);
            db.EmployeesSkills.Remove(employeeSkill);
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
