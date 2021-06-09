using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebProject1.DAL;
using System.Dynamic;

namespace WebProject1.Controllers
{
    public class actorsController : Controller
    {
        private movieDatabaseEntities db = new movieDatabaseEntities();

        public ActionResult searchActor()
        {
            dynamic mr = new ExpandoObject();
       
            List<actor> actors = (List<actor>)TempData["actorSearchResult"];
            mr.Actors = actors;
            return View(mr);
        }

        // GET: actors
        public ActionResult Index()
        {
           
              return View(db.actor.ToList());
        }

        // GET: actors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            actor actor = db.actor.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            List<actorInMovie> actorInMovie = (from am in db.actorInMovie
                                where am.actorid == id
                                select am).ToList();
            List<movies> movies = new List<movies>();
            List<rating> ratings = new List<rating>();
            if(actorInMovie.Count != 0)
            {
                foreach(actorInMovie a in actorInMovie)
                {
                    movies acMovie = db.movies.Find(a.movieid);
                    movies.Add(acMovie);
                }
                ratings =   (from r in db.rating
                             select r).ToList();

            }



            dynamic mr = new ExpandoObject();
            mr.Actor = actor;
            mr.Movies = movies;
            mr.Ratings = ratings;
            return View(mr);
        }

        // GET: actors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create([Bind(Include = "actorid,actorname,gender,overview,imageurl")] actor actor)
        {
            if (ModelState.IsValid)
            {
                db.actor.Add(actor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actor);
        }

        // GET: actors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            actor actor = db.actor.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "actorid,actorname,gender,overview,imageurl")] actor actor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }

        // GET: actors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            actor actor = db.actor.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: actors/Delete/5
        [HttpPost]
        
        public ActionResult DeleteConfirmed(string actorID)
        {
            int id = Int32.Parse(actorID);
            
            List<actorInMovie> am = db.actorInMovie.Where(x => x.actorid == id).ToList();
            if (am.Count != 0)
            {
                db.actorInMovie.RemoveRange(am);
                db.SaveChanges();
            }

            actor actor = db.actor.Find(id);
            db.actor.Remove(actor);
            db.SaveChanges();
            return Json(new
            {
                Msg = "Actor Removed successfullly"

            }, JsonRequestBehavior.AllowGet);
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
