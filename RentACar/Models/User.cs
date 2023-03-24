using Microsoft.AspNetCore.Identity;

namespace RentACar.Models
{
    public class User : IdentityUser
    {
        public override string Id { get; set; }
        public string Password { get; set; }
        public override string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EGN { get; set; }
        public override string PhoneNumber { get; set; }
        public override string Email { get; set; }
    }
}
