using BL.IdentityFramework;
using Domain.IdentityFramework;
using Microsoft.AspNet.Identity.Owin;
using PB_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PB_MVC.Controllers
{
  [Authorize(Roles = "Admin,SuperAdmin")]
  [RequireHttps]
  public class AdminController : Controller
  {
    public ActionResult Index()
    {
      List<ApplicationUser> users = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.ToList();
      List<AdminUserViewModel> userViewModels = new List<AdminUserViewModel>();
      foreach (var user in users)
      {
        userViewModels.Add(new AdminUserViewModel
        {
          Email = user.Email,
        }
        );
      }
      return View(userViewModels);
    }
  }
}