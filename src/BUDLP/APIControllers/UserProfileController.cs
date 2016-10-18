﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BUDLP.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BUDLP.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using BUDLP.Models.TopicModels;

namespace BUDLP.APIControllers
{
    public class UserProfileController : Controller
    {
        private UserManager<AuthenticatedUser> _userManager;
        private ApplicationDbContext _appDb;
        private RoleManager<IdentityRole> _roleManager;
        private PlatformDbContext _ctx;

        public UserProfileController(PlatformDbContext ctx, ApplicationDbContext appDb, UserManager<AuthenticatedUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _ctx = ctx;
            _appDb = appDb;
            _roleManager = roleManager;
        }

        [HttpPost("api/profile/create")]
        public async Task<JsonResult> GetTopics([FromBody] Profile payload)
        {
            var user = new AuthenticatedUser { UserName = payload.Email, Email = payload.Email, FirstName = payload.FullName, LastName = "", IsStaff = false, IsActive = true, IsSuperUser = false };
            var result = await _userManager.CreateAsync(user, payload.Password);

            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                //await _signInManager.SignInAsync(user, isPersistent: false);
                //_logger.LogInformation(3, "User created a new account with password.");
                //return RedirectToLocal(returnUrl);

                foreach (var top in payload.ProfileTopics)
                {
                    _ctx.UserProfileTopics.Add(new UserProfileTopic()
                    {
                        ToLearn = top.ToLearn,
                        TopicId = top.TopicId,
                        UserProfileId = user.Id
                    });
                }

                _ctx.SaveChanges();
            }

            return new JsonResult("Account created.");

            
        }

        public class Profile
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public List<Topics> ProfileTopics { get; set; }
        }

        public class Topics : Topic
        {
            public bool ToLearn { get; set; }
        }
    }
}