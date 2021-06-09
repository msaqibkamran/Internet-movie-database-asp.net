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
using System.Dynamic;

namespace WebProject1.Controllers
{
    public class watch_listController : Controller
    {
        private movieDatabaseEntities db = new movieDatabaseEntities();

        [HttpPost]
        public JsonResult myCreate(string userId, string movieID)
        {
            watch_list wl = new watch_list();
            wl.movieid = Int32.Parse(movieID);
            wl.id = 12;
            wl.userid = Int32.Parse(userId);

            // if already added
            var alreadyinList = (from l in db.watch_list
                                 where l.userid == wl.userid && l.movieid == wl.movieid
                                 select l).ToList();
            if (alreadyinList.Count == 0)
            {
                db.watch_list.Add(wl);
                try
                {
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
                return Json(new
                {
                    Msg = "Movie Added to Watch List"

                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    Msg = "Movie Already in List"

                }, JsonRequestBehavior.AllowGet);
            }



            

        }

        // GET: watch_list
        public ActionResult Index()
        {
            dynamic mr = new ExpandoObject();
           
            List<watch_list> watch_list = (from w in db.watch_list
                             where w.userid == 1004
                             select w).ToList();
            var userInfo = from u in db.user
                           where u.userid == 1004
                           select u;
           

            List<movies> requiredMovies = new List<movies>();
            foreach(var mo in watch_list)
            {
                List<movies> m = db.movies.Where(x => x.movieid == (mo.movieid)).ToList();
                

                if (m.Count != 0)
                {
                    movies movi = m.First();
                    requiredMovies.Add(movi);
                }
                
            }

            var allMovieRating = from rat in db.rating
                                 select rat;

            mr.moviesInWatchList = requiredMovies;
            mr.userDetails = userInfo.First();
            mr.Ratings = allMovieRating.ToList();

            return View(mr);
        }

        // GET: watch_list/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            watch_list watch_list = db.watch_list.Find(id);
            if (watch_list == null)
            {
                return HttpNotFound();
            }
            return View(watch_list);
        }

        // GET: watch_list/Create
        public ActionResult Create()
        {
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name");
            ViewBag.userid = new SelectList(db.user, "userid", "username");
            return View();
        }

        // POST: watch_list/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,userid,movieid")] watch_list watch_list)
        {
            if (ModelState.IsValid)
            {
                db.watch_list.Add(watch_list);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", watch_list.movieid);
            ViewBag.userid = new SelectList(db.user, "userid", "username", watch_list.userid);
            return View(watch_list);
        }

        // GET: watch_list/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            watch_list watch_list = db.watch_list.Find(id);
            if (watch_list == null)
            {
                return HttpNotFound();
            }
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", watch_list.movieid);
            ViewBag.userid = new SelectList(db.user, "userid", "username", watch_list.userid);
            return View(watch_list);
        }

        // POST: watch_list/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,userid,movieid")] watch_list watch_list)
        {
            if (ModelState.IsValid)
            {
                db.Entry(watch_list).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", watch_list.movieid);
            ViewBag.userid = new SelectList(db.user, "userid", "username", watch_list.userid);
            return View(watch_list);
        }

        // GET: watch_list/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            watch_list watch_list = db.watch_list.Find(id);
            if (watch_list == null)
            {
                return HttpNotFound();
            }
            return View(watch_list);
        }

        // POST: watch_list/Delete/5
        //  [HttpPost, ActionName("Delete")]
        [HttpPost]
        public JsonResult DeleteConfirmed(string movieID, string userId)
        {
            int movie = Int32.Parse(movieID);
            int user = Int32.Parse(userId);

            var mo = (from m in db.watch_list
                        where m.movieid == movie && m.userid == user
                        select m).ToList();
            watch_list movieTobeDeleted = mo.First();

              watch_list watch_list = db.watch_list.Find(movieTobeDeleted.id);
            db.watch_list.Remove(watch_list);
            db.SaveChanges();

   
            
            return Json(new
            {
                Msg = "Removed from Watch List successfullly"
                
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
