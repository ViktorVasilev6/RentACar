using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentACar.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentACar.Validator
{
    public class UserValidator : IUserValidator<User>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            bool email = await manager.Users
                .AnyAsync(x => x.Email == user.Email);
            if(email)
            {
                errors.Add(new IdentityError
                {
                    Code = "0001",
                    Description = "Email already taken!"
                });
            }
            bool EGN = await manager.Users
                .AnyAsync(x => x.EGN == user.EGN);
            if(EGN)
            {
                errors.Add(new IdentityError
                {
                    Code = "0002",
                    Description = "EGN already taken!"
                });
            }
            if (email || EGN)
            {
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }
    }
}
