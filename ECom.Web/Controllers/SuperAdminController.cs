using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ECom.Web.Models;
using ECom.Web.Models.SuperAdminViewModels;

namespace ECom.Web.Controllers
{
    [Authorize(Roles = "SuperAdmins")]
    public class SuperAdminController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IUserValidator<ApplicationUser> userValidator;
        private IPasswordValidator<ApplicationUser> passwordValidator;
        private IPasswordHasher<ApplicationUser> passwordHasher;

        private ApplicationUser testUser = new ApplicationUser
        {
            UserName = "TestTestForPassword",
            Email = "testForPassword@test.test"
        };

        public SuperAdminController(UserManager<ApplicationUser> userMgr,
            IUserValidator<ApplicationUser> userValid, IPasswordValidator<ApplicationUser> passValid,
            IPasswordHasher<ApplicationUser> passHasher)
        {
            userManager = userMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passHasher;
        }

        // GET: /<controller>/
        public ViewResult Index()
        {
            return View(userManager.Users);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVm createVm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = createVm.Email,
                    Email = createVm.Email,
                    //extended properties
                    FirstName = createVm.FirstName,
                    LastName = createVm.LastName,
                    AvatarURL = "/images/user.png",
                    DateRegistered = DateTime.UtcNow.ToString(),
                    Position = "",
                    NickName = "",
                };

                IdentityResult result = await userManager.CreateAsync(user, createVm.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(createVm);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.TryAddModelError("", error.Description);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View("Index", userManager.Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // the names of its parameters must be the same as the property of the User class if we use asp-for in the view
        // otherwise form values won't be passed properly
        public async Task<IActionResult> Edit(string id, string userName, string email)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Validate UserName and Email 
                user.UserName = userName; // UserName won't be changed in the database until UpdateAsync is executed successfully
                user.Email = email;
                IdentityResult validUseResult = await userValidator.ValidateAsync(userManager, user);
                if (!validUseResult.Succeeded)
                {
                    AddErrors(validUseResult);
                }

                // Update user info
                if (validUseResult.Succeeded)
                {
                    // UpdateAsync validates user info such as UserName and Email except password since it's been hashed 
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "SuperAdmin");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            ;

            return View(user);
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, string password)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Validate password
                // Step 1: using built in validations
                IdentityResult passwordResult = await userManager.CreateAsync(testUser, password);
                if (passwordResult.Succeeded)
                {
                    await userManager.DeleteAsync(testUser);
                }
                else
                {
                    AddErrors(passwordResult);
                }
                /* Step 2: Because of DI, IPasswordValidator<User> is injected into the custom password validator. 
                   So the built in password validation stop working here */
                IdentityResult validPasswordResult = await passwordValidator.ValidateAsync(userManager, user, password);
                if (validPasswordResult.Succeeded)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                }
                else
                {
                    AddErrors(validPasswordResult);
                }

                // Update user info
                if (passwordResult.Succeeded && validPasswordResult.Succeeded)
                {
                    // UpdateAsync validates user info such as UserName and Email except password since it's been hashed 
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "SuperAdmin");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View(user);
        }
    }
}
