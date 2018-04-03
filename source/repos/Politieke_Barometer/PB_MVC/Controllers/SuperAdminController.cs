using BL.IdentityFramework;
using Domain.IdentityFramework;
using Microsoft.AspNet.Identity.Owin;
using PB_MVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PB_MVC.Controllers
{
  [Authorize(Roles = "SuperAdmin")]
  [RequireHttps]
  public class SuperAdminController : Controller
    {
    public ActionResult Index()
    {
      List<ApplicationUser> users = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.ToList();
      List<SuperAdminUserViewModel> userViewModels = new List<SuperAdminUserViewModel>();
      foreach (var user in users)
      {
        userViewModels.Add(new SuperAdminUserViewModel
        {
          Email = user.Email,
        }
        );
      }
      return View(userViewModels);
    }
  }
}