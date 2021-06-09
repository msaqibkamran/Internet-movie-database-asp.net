using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebProject1.DAL;

namespace WebProject1.Controllers
{
    public class adminsController : Controller
    {
        private movieDatabaseEntities db = new movieDatabaseEntities();

        // GET: admins
        public ActionResult Index()
        {
            dynamic mr = new ExpandoObject();

            

            var allMovieRating = from rat in db.rating
                                 select rat;
            var allMovies = from m in db.movies
                            select m;
            var admin = (from a in db.admin
                        where a.adminname.Equals("Saqib")
                        select a).ToList();

            mr.Movies = allMovies.ToList();
            mr.Ratings = allMovieRating.ToList();
            mr.Admin = admin.First();

            return View(mr);

        }

        public ActionResult adminActors()
        {
            dynamic mr = new ExpandoObject();



            
            var admin = (from a in db.admin
                         where a.adminname.Equals("Saqib")
                         select a).ToList();
            var actors = (from a in db.actor
                          select a).ToList();
            mr.Actors = actors;
            mr.Admin = admin.First();

            return View(mr);

        }
        // GET: admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "adminid,adminname,password")] admin admin)
        {
            if (ModelState.IsValid)
            {
                db.admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "adminid,adminname,password")] admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            admin admin = db.admin.Find(id);
            db.admin.Remove(admin);
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
