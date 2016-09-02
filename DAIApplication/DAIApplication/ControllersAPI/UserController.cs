using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAIApplication.Data.Database;
using DAIApplication.Models;
using DAIApplication.Services.UserService;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAIApplication.ControllersAPI
{
    public class UserController : ApiController
    {
        private UserService _userService;
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public UserController()
        {
            _userService = new UserService();
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        // GET api/<controller>
        public object Get()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                return new
                {
                    success = false,
                    message = "No logged in user!",
                    data = new {}
                };
            }

            var userRole = _userService.GetUserRole(user.Email);
            var userName = _userService.GetUsername(user.Email);
            return new
            {
                success = true,
                data = new
                {
                    UserName = userName,
                    user.Email,
                    UserRole = userRole
                }
            };
        }

        [HttpGet]
        [Route("api/userprofile")]
        public object GetUserProfile()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = _userService.GetUserProfile(userId);


            return new
            {
                success = true,
                data = userProfile
            };
        }

        [HttpPut]
        [Route("api/userprofile")]
        public object UpdateUserProfile([FromBody] UserProfile userProfile)
        {
            var userId = User.Identity.GetUserId();

            var result = _userService.UpdateUserProfile(userId, userProfile);
            
            return new
            {
                success = result
            };
        }

        [HttpGet]
        [Route("api/user-history")]
        public object GetAdminTestInfo()
        {
            var userId = User.Identity.GetUserId();

            var userHistory = _userService.GetUserHistory(userId);

            return new
            {
                success = true,
                data = userHistory
            };
        }

    }
}
