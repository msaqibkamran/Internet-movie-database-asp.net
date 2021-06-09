using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject1.DAL;
using WebProject1.DAL.ViewModels;
using WebProject1.Models;
using System.Dynamic;

namespace WebProject1.Controllers
{
    public class HomeController : Controller
    {
        private movieDatabaseEntities db = new movieDatabaseEntities();
        private List<user> loggedInUsers = new List<user>();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {

            String str = (String)TempData["userNotFound"];
            return View((object)str);
        }

        public ActionResult SignUp()
        {
            ViewBag.Message = "SignUp page.";

            return View();
        }

        public ActionResult MainPage(string username, string password)
        {

            ViewBag.Message = "Login page.";
            var user = (from u in db.user
                       where u.username == username && u.password == password
                       select u).ToList();
            //var admin = from a in db.admin
            //            where a.adminname == username && a.password == password
            //            select a;
            
            if (user.Count != 0)
            {
               // loggedInUsers.Add(user);
                if (Session["LoggedInUsers"] == null)
                {
                    Session["LoggedInUsers"] = user;
                }
                else
                {
                    Session["LoggedInUsers"] = user;
                }
                
                
            }
            else
            {
                var noUserFound = "No User Found";
                TempData["userNotFound"] = noUserFound;
                return RedirectToAction("Login", "Home");
            }
            ViewBag.Message = "Main page.";


            dynamic mr = new ExpandoObject();
            var movies = (from m in db.movies
                         
                         select m).ToList();

            var rat = (from r in db.rating
                      select r).ToList();

            var genre = (from g in db.genre
                       select g).ToList();

            List<movies> topRated = new List<movies>();

            double ? sum = 0;
            int count = 0;
            foreach(movies m in movies)
            {
                sum = 0;
                count = 0;
                foreach (rating r in rat)
                {
                    if (r.movieid == m.movieid)
                    {
                        sum = sum + r.stars;
                        count++;
                    }
                }
                
                if((sum / count) >= 7)
                {
                    topRated.Add(m);
                }
            }

            var favMovies = (from m in db.movies
                            where m.movieid == 1 || m.movieid == 2 || m.movieid == 4 || m.movieid == 5 || m.movieid == 6 || m.movieid == 7 || m.movieid == 8 || m.movieid == 11
                            select m).ToList();
            mr.topRatedMovies = topRated;
            mr.favMovies = favMovies;
            mr.Ratings = rat;
            mr.Genre = genre;


            return View(mr);
        }

        public ActionResult Movielist()
        {
            ViewBag.Message = "Movie List.";

            return View();
        }

        public ActionResult movieSingle()
        {
            ViewBag.Message = "Movie Single.";

            return View();
        }

        public ActionResult Celebritylist()
        {
            ViewBag.Message = "Celebrity List.";
            //var actor = from act in WebProject1.DAL.actor
            //            where act.actorid = 1
            //            select act;

            return View();
        }

        public ActionResult SignupNew()
        {
            ViewBag.Message = "SignupNew";
            return View();
        }
        
    }
}