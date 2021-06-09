using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebProject1.DAL;

namespace WebProject1.Controllers
{
    public class actorInMoviesController : Controller
    {
        private movieDatabaseEntities db = new movieDatabaseEntities();

        // GET: actorInMovies
        public ActionResult Index()
        {
            var actorInMovie = db.actorInMovie.Include(a => a.actor).Include(a => a.movies);
            return View(actorInMovie.ToList());
        }

        // GET: actorInMovies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            actorInMovie actorInMovie = db.actorInMovie.Find(id);
            if (actorInMovie == null)
            {
                return HttpNotFound();
            }
            return View(actorInMovie);
        }

        // GET: actorInMovies/Create
        public ActionResult Create()
        {
            ViewBag.actorid = new SelectList(db.actor, "actorid", "actorname");
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name");
            return View();
        }

        // POST: actorInMovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,actorid,movieid")] actorInMovie actorInMovie)
        {
            if (ModelState.IsValid)
            {
                db.actorInMovie.Add(actorInMovie);
                db.SaveChanges();
                return RedirectToAction("Index", "admins");
            }

            ViewBag.actorid = new SelectList(db.actor, "actorid", "actorname", actorInMovie.actorid);
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", actorInMovie.movieid);
            return View(actorInMovie);
        }

        // GET: actorInMovies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            actorInMovie actorInMovie = db.actorInMovie.Find(id);
            if (actorInMovie == null)
            {
                return HttpNotFound();
            }
            ViewBag.actorid = new SelectList(db.actor, "actorid", "actorname", actorInMovie.actorid);
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", actorInMovie.movieid);
            return View(actorInMovie);
        }

        // POST: actorInMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,actorid,movieid")] actorInMovie actorInMovie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actorInMovie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.actorid = new SelectList(db.actor, "actorid", "actorname", actorInMovie.actorid);
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", actorInMovie.movieid);
            return View(actorInMovie);
        }

        // GET: actorInMovies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            actorInMovie actorInMovie = db.actorInMovie.Find(id);
            if (actorInMovie == null)
            {
                return HttpNotFound();
            }
            return View(actorInMovie);
        }

        // POST: actorInMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            actorInMovie actorInMovie = db.actorInMovie.Find(id);
            db.actorInMovie.Remove(actorInMovie);
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
