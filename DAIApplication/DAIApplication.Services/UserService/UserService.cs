using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAIApplication.Data.Database;

namespace DAIApplication.Services.UserService
{
    public class UserService
    {
        private DbEntities _dbEntities;

        public UserService()
        {
            _dbEntities = new DbEntities();
        }

        public object RegisterUser(UserProfile userProfile)
        {
            if (userProfile.Username == null || userProfile.UserId == null ||
                userProfile.Email == null)
                return new
                {
                    success = false,
                    message = "Field missing"
                };

            _dbEntities.UserProfiles.Add(userProfile);
            _dbEntities.SaveChanges();

            return new
            {
                success = true,
                message = "Success",
                data = new { }
            };
        }

        public string GetUserRole(string email)
        {
            var user = _dbEntities.AspNetUsers.Include("AspNetRoles").FirstOrDefault(f => f.Email == email);
            var role = user.AspNetRoles.First();
            return role.Id;
        }

        public string GetUsername(string email)
        {
            var user = _dbEntities.UserProfiles.FirstOrDefault(f => f.Email == email);
            var username = user.Username;
            return username;
        }

        public object GetUserProfile(string userId)
        {
            UserProfile userProfile = _dbEntities.UserProfiles.FirstOrDefault(f => f.UserId == userId);

            return new
            {
                userProfile.FirstName,
                userProfile.LastName,
                userProfile.Email,
                userProfile.PhoneNo,
                userProfile.Country,
                userProfile.Sex,
                userProfile.Username
            };
        }

        public bool UpdateUserProfile(string userId, UserProfile userProfile)
        {
            UserProfile originalUserProfile = _dbEntities.UserProfiles.FirstOrDefault(f => f.UserId == userId);

            originalUserProfile.FirstName = userProfile.FirstName;
            originalUserProfile.LastName = userProfile.LastName;
            originalUserProfile.Country = userProfile.Country;
            originalUserProfile.PhoneNo = userProfile.PhoneNo;
            originalUserProfile.Sex = userProfile.Sex;
            _dbEntities.SaveChanges();

            return true;
        }
    }
}
