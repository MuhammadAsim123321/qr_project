using Microsoft.AspNetCore.Identity;

namespace Identity_Login.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsDeleted { get; set; } = false ;
        //public string? CompanyName { get; set; }
        //public string? Address1 { get; set; }
        //public string? Address2 { get; set; }
        //public string? PostCode { get; set; }
        //public string? City { get; set; }
        //public string? Country { get; set; }
        //public string? TVAnumber { get; set; }
        //public override string? PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

    }
}
