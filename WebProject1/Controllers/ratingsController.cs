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
    public class ratingsController : Controller
    {
        private movieDatabaseEntities db = new movieDatabaseEntities();

        [HttpPost]
        public JsonResult myCreate(string stars, string userId, string movieID)
        {

           

            rating rati = new rating();
            rati.movieid = Int32.Parse(movieID);
            rati.date = new DateTime();
            rati.id = 12;
            rati.stars = (double?)Int32.Parse(stars);
            rati.userid = Int32.Parse(userId);
           


            if (ModelState.IsValid)
            {
                // delete old comment
                var rat1 = (from r in db.rating
                           where r.userid == rati.userid && r.movieid == rati.movieid
                           select r).ToList();
                if(rat1.Count != 0)
                {
                    rating deleteRating = rat1.First();
                    db.rating.Remove(deleteRating);
                    db.SaveChanges();
                }
                
                

                // new rating 
                db.rating.Add(rati);



            }
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

            var rat = (from r in db.rating
                       where r.movieid == rati.movieid
                       select r).ToList();
            double avgRating = 0;
            foreach (var r in rat)
            {
                avgRating += r.stars.Value;
            }
            int countOfRating = rat.Count();
            avgRating = (avgRating / countOfRating);


            double avg = Convert.ToDouble(String.Format("{0:0.#}", avgRating));
            avgRating = avg;

            return Json(new
            {
               Msg = "Success",
               avgRat = avgRating,
               ratingCount = countOfRating
            }, JsonRequestBehavior.AllowGet);

        }



        // GET: ratings
        public ActionResult Index()
        {
            var rating = db.rating.Include(r => r.movies).Include(r => r.user);
            return View(rating.ToList());
        }

        // GET: ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rating rating = db.rating.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: ratings/Create
        public ActionResult Create()
        {
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name");
            ViewBag.userid = new SelectList(db.user, "userid", "username");
            return View();
        }

        // POST: ratings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,movieid,userid,stars,date")] rating rating)
        {
            if (ModelState.IsValid)
            {
                db.rating.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", rating.movieid);
            ViewBag.userid = new SelectList(db.user, "userid", "username", rating.userid);
            return View(rating);
        }

        // GET: ratings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rating rating = db.rating.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", rating.movieid);
            ViewBag.userid = new SelectList(db.user, "userid", "username", rating.userid);
            return View(rating);
        }

        // POST: ratings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,movieid,userid,stars,date")] rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", rating.movieid);
            ViewBag.userid = new SelectList(db.user, "userid", "username", rating.userid);
            return View(rating);
        }

        // GET: ratings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rating rating = db.rating.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rating rating = db.rating.Find(id);
            db.rating.Remove(rating);
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
