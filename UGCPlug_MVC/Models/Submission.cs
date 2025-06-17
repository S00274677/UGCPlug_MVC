using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UGCPlug_MVC.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<System.DateTime> DateTaken { get; set; }
        public string FileUrl { get; set; }
        public bool ConsentGiven { get; set; }
        public string BusinessId { get; set; }
        public System.DateTime DateSubmitted { get; set; }
    }
}