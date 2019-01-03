using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMAPP.Models
{
    public class Sponsor
    {
        public int Id { get; set; }
        [Display(Name = "Sponsor's Name:")]
        public string Name { get; set; }

        [Display(Name = "Link :")]
        public string Url { get; set; }

        [Display(Name = "Username or Email :")]
        public string Username { get; set; }

        [Display(Name = "Password:")]
        public string Password { get; set; }

        [Display(Name = "Username or Email html Tag :")]
        public string UsernameTag { get; set; }

        [Display(Name = "Password html Tag:")]
        public string PasswordTag { get; set; }

        [Display(Name = "login button html Tag:")]
        public string SubmitTag { get; set; }

        [Display(Name = "Username or Email html TagType :")]
        public string UsernameTagType { get; set; }

        [Display(Name = "Password html TagType :")]
        public string PasswordTagType { get; set; }

        [Display(Name = "login button html TagType:")]
        public string SubmitTagType { get; set; }

        public bool isDeleted { get; set; }

    }
}