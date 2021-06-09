using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebProject1.DAL;

namespace WebProject1.Controllers
{
    public class genresController : Controller
    {
        private movieDatabaseEntities db = new movieDatabaseEntities();

        // GET: genres
        public ActionResult Index()
        {
            var genre = db.genre.Include(g => g.movies);
            return View(genre.ToList());
        }

        // GET: genres/Details/5

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            genre genre = db.genre.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // GET: genres/Create
        public ActionResult Create()
        {
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name");
            
            return View();
        }

        // POST: genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "genreid,movieid,genrename")] genre genre)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    
                    db.genre.Add(genre);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
               
                return RedirectToAction("Index", "admins");
            }

            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", genre.movieid);
            return View(genre);
        }

        // GET: genres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            genre genre = db.genre.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", genre.movieid);
            return View(genre);
        }

        // POST: genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "genreid,movieid,genrename")] genre genre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", genre.movieid);
            return View(genre);
        }

        // GET: genres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            genre genre = db.genre.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // POST: genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            genre genre = db.genre.Find(id);
            db.genre.Remove(genre);
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
