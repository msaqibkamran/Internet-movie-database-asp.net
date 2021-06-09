using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebProject1.DAL;
using WebProject1.DAL.ViewModels;
using WebProject1.Models;
using System.Dynamic;
using System.Data.Entity.Validation;

namespace WebProject1.Controllers
{
    public class moviesController : Controller
    {
        private movieDatabaseEntities db = new movieDatabaseEntities();


        public ActionResult searchMovie(string searchText, string options)
        {

            var rating = from rat in db.rating
                         select rat;
            var movies = db.movies.Include(m => m.genre);
            dynamic mr = new ExpandoObject();
            mr.Ratings = rating.ToList();
            if (options == "byName" && searchText != "")
            {
                var mov = db.movies.Where(x => x.name.Contains(searchText) || searchText == null).ToList();
                mr.Movies = mov;
                return View(mr);
            }
            else if (options == "byGenre" && searchText != "")
            {
                var gen = db.genre.Where(x => x.genrename.Contains(searchText) || searchText == null);


                var relatedmovies3 = from m in db.movies
                                     from g in gen
                                     where m.movieid == g.movieid

                                     select m;
                mr.Movies = relatedmovies3.ToList();
                return View(mr);
            }
            else if(options == "byActor" && searchText != "")
            {
                List<actor> actorSearch = (db.actor.Where(x => x.actorname.Contains(searchText) || searchText == null)).ToList();
                TempData["actorSearchResult"] = actorSearch;
                return RedirectToAction("searchActor", "actors");
            }
            else
            {
                return RedirectToAction("notFound");
            }
            
        }

        public ActionResult notFound()
        {

            return View();
        }

        // GET: movies


        public ActionResult Index()
        {
            var rating = from rat in db.rating
                         select rat;
            var movies = db.movies.Include(m => m.genre);
            dynamic mr = new ExpandoObject();
            mr.Movies = movies.ToList();
            mr.Ratings = rating.ToList();

            return View(mr);
        }



        // GET: movies/Details/5
        // [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            movies movies = db.movies.Find(id);
            if (movies == null)
            {
                return HttpNotFound();
            }
            var genre = from g in db.genre
                        where g.movieid == movies.movieid
                        select g;
            //var mov = db.movies.Include(m => m.genre);

            var rm1 = from g in db.genre
                      where g.movieid == id
                      select new
                      {
                          g.genrename
                      };


            var actors = from a in db.actorInMovie
                         join ac in db.actor on a.actorid equals ac.actorid
                         where a.movieid == id
                         select ac;

            var com = (from c in db.comment
                       join us in db.user on c.userid equals us.userid
                       where c.movieid == id
                       select new
                       {
                           c.movieid,
                           c.commentstatement,
                           us.username,
                           us.userid

                       });

            var comments = (from d in com
                            join ra in db.rating on d.userid equals ra.userid
                            where ra.movieid == id
                            select new
                            {
                                id,
                                d.userid,
                                d.username,
                                d.commentstatement,
                                ra.stars

                            });


            List<userComments> commentsList = new List<userComments>();
            foreach (var item in comments)
            {
                userComments uc = new userComments();
                uc.stars = item.stars.Value;
                uc.userid = item.userid;
                uc.username = item.username;
                uc.commentstatement = item.commentstatement;
                commentsList.Add(uc);

            }

            var rati = from rat in db.rating
                       where rat.movieid == id
                       select rat;
            var rating = rati.ToList();
            double avgRating = 0;
            foreach (var r in rating)
            {
                avgRating += r.stars.Value;
            }
            int countOfRating = rating.Count();
            avgRating = (avgRating / countOfRating);
            
           
            double avg = Convert.ToDouble(String.Format("{0:0.#}", avgRating));
            avgRating = avg;




            var relatedmovies = from g in db.genre
                                where g.movieid == id
                                select new
                                {
                                    g.genreid
                                };


            var relatedmovies2 = from g in db.genre
                                 from r in relatedmovies
                                 where g.genreid == r.genreid && g.movieid != id
                                 select new
                                 {
                                     g.movieid
                                 };

            var relatedmovies3 = from m in db.movies
                                 from r in relatedmovies2
                                 where m.movieid == r.movieid

                                 select m;

            var allMovieRating =    from rat in db.rating
                                    select rat;

            var userRated = (from r in db.rating
                            where r.movieid == id && r.userid == 1004
                            select r).ToList();
            double? userStars = 0;
            if (userRated.Count != 0)
            {
                var urated = userRated.First();
                userStars = urated.stars;
            }
            

            dynamic mr = new ExpandoObject();
            mr.Movies = movies;
            mr.Genre = genre.ToList();
            mr.Actors = actors.ToList();
            mr.Comments = commentsList;
            mr.RatingCount = countOfRating;
            mr.RatingAverage = avgRating;
            mr.RelatedMovies = relatedmovies3.ToList();
            mr.Ratings = allMovieRating.ToList();
            mr.userRating = userStars;

            return View(mr);
        }

        // GET: movies/Create


       // [Authorize(Users = "saqib", Roles = "admin")]
        public ActionResult Create()
        {

            admin a = Session["LoggedInUsers"] as admin;
            ViewBag.genre = new SelectList(db.genre, "genreid", "genrename");
            return View();
        }

        // POST: movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "movieid,name,release_date,duration,directorname,overview,pictureurl,trailerurl")] movies movies)
        {
            if (ModelState.IsValid)
            {
                db.movies.Add(movies);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.genre = new SelectList(db.genre, "genreid", "genrename", movies.genre);
            return View(movies);
        }

        // GET: movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            movies movies = db.movies.Find(id);
            if (movies == null)
            {
                return HttpNotFound();
            }
            ViewBag.genre = new SelectList(db.genre, "genreid", "genrename", movies.genre);
            return View(movies);
        }

        // POST: movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "movieid,name,release_date,duration,directorname,overview,genre,pictureurl,trailerurl")] movies movies)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movies).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.genre = new SelectList(db.genre, "genreid", "genrename", movies.genre);
            return View(movies);
        }

        // GET: movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            movies movies = db.movies.Find(id);
            if (movies == null)
            {
                return HttpNotFound();
            }
            return View(movies);
        }

        // POST: movies/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(string movieID)
        {
            int id = Int32.Parse(movieID);
            List<genre> g = db.genre.Where(x => x.movieid == id).ToList();
            if(g.Count != 0)
            {
                db.genre.RemoveRange(g);
                db.SaveChanges();
            }

            List<actorInMovie> am = db.actorInMovie.Where(x => x.movieid == id).ToList();
            if(am.Count != 0)
            {
                db.actorInMovie.RemoveRange(am);
                db.SaveChanges();
            }
            

            List<comment> cm = db.comment.Where(x => x.movieid == id).ToList();
            if(cm.Count != 0)
            {
                db.comment.RemoveRange(cm);
                db.SaveChanges();
            }
            

            List<rating> rt = db.rating.Where(x => x.movieid == id).ToList();
            if(rt.Count != 0)
            {
                db.rating.RemoveRange(rt);
                db.SaveChanges();
            }
           


            movies movies = db.movies.Find(id);

            try
            {
                db.movies.Remove(movies);
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
                Msg = "Movie Removed successfullly"

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
