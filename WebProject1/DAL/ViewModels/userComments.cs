using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject1.DAL.ViewModels
{
    public class userComments
    {
        //public userComments(userComments s)
        //{
        //    this.commentstatement = s.commentstatement;
        //    this.movieid = s.movieid;
        //    this.stars = s.stars;
        //    this.userid = s.userid;
        //    this.username = s.username;
        //}
        public userComments()
        {

        }
        public  int movieid { get; set; }
        public int userid { get; set; }
        public string username { get; set; }
        public string commentstatement { get; set; }
        public double stars { get; set; }


                       
    }
}