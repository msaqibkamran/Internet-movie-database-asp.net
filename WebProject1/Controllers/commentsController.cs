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
using System.Dynamic;
using System.Web.Services;
using System.Data.Entity.Validation;

namespace WebProject1.Controllers
{
    public class commentsController : Controller
    {
        private movieDatabaseEntities db = new movieDatabaseEntities();

        // GET: comments
        public ActionResult Index()
        {
            var comment = db.comment.Include(c => c.movies).Include(c => c.user);
            return View(comment.ToList());
        }

        // GET: comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: comments/Create
        public ActionResult Create(int? id)
        {
           
            
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name");
            ViewBag.userid = new SelectList(db.user, "userid", "username");
            
            return View();
        }

        // POST: comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "commentid,movieid,userid,commentstatement,date")] comment comment)
        {
            if (ModelState.IsValid)
            {
                db.comment.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", comment.movieid);
            ViewBag.userid = new SelectList(db.user, "userid", "username", comment.userid);
            

            return View(comment);
        }

        [HttpPost]
      //  [WebMethod]
        public JsonResult myCreate(string comment, string userId, string movieID)
        {


            comment comm = new comment();
            comm.movieid = Int32.Parse(movieID);
            comm.date = new DateTime();
            comm.commentid = 12;
            comm.commentstatement = comment;
            comm.userid = Int32.Parse(userId);

            var userData = (from us in db.user
                           where us.userid == comm.userid
                           select us);
            var userDataFirst = userData.First();

            
            var stars =(from r in db.rating
                        where r.movieid == comm.movieid && r.userid == comm.userid
                        select r).ToList();
            rating star = null ;
            if(stars.Count == 0)
            {
                star = new rating();
                star.id = 12;
                star.movieid = comm.movieid;
                star.userid = comm.userid;
                star.date = new DateTime();
                star.stars = 9;
                db.rating.Add(star);
                db.SaveChanges();
            }
            else
            {
                star = stars.First();
            }
            
                   
            // data for sending to ajax call
            userComments uc = new userComments();
            uc.commentstatement = comm.commentstatement;
            uc.movieid = (int)star.movieid;
            uc.userid = (int)star.userid;
            uc.stars = (double)star.stars;
            uc.username = userDataFirst.username;

            if (ModelState.IsValid)
            {
                db.comment.Add(comm);
               
             
               
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


            return Json(new {
                userid = uc.userid,
                movieid = uc.movieid,
                commentstatement = uc.commentstatement,
                stars = uc.stars,
                username = uc.username
                }, JsonRequestBehavior.AllowGet);

        }

        // GET: comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", comment.movieid);
            ViewBag.userid = new SelectList(db.user, "userid", "username", comment.userid);
            return View(comment);
        }

        // POST: comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "commentid,movieid,userid,commentstatement,date")] comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.movieid = new SelectList(db.movies, "movieid", "name", comment.movieid);
            ViewBag.userid = new SelectList(db.user, "userid", "username", comment.userid);
            return View(comment);
        }

        // GET: comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            comment comment = db.comment.Find(id);
            db.comment.Remove(comment);
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
