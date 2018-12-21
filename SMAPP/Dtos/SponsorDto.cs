using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMAPP.Dtos
{
    public class SponsorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UsernameTag { get; set; }
        public string PasswordTag { get; set; }
        public string SubmitTag { get; set; }
        public bool isDeleted { get; set; }
    }
}