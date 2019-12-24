using Microsoft.AspNetCore.Identity;
using System;

namespace TabSanat.Model
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public DateTime? BirthDate { get; set; } 
    }
}
