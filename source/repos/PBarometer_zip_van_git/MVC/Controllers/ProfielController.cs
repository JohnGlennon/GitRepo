using BL.IdentityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
  [Authorize]
  public partial class ProfielController : Controller
  {
    private ApplicationSignInManager _signInManager;
    private ApplicationUserManager _userManager;

    public ProfielController()
    {
    }

    public ProfielController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
    {
      UserManager = userManager;
      SignInManager = signInManager;
    }

    public ApplicationSignInManager SignInManager
    {
      get
      {
        return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
      }

      private set
      {
        _signInManager = value;
      }
    }

    public ApplicationUserManager UserManager
    {
      get
      {
        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
      }
      private set
      {
        _userManager = value;
      }
    }

    public virtual ActionResult Index()
    {
      return View();
    }

    //
    // GET: /Profiel/Index
    [HttpPost]
    public virtual async Task<ActionResult> Index(ManageMessageId? message)
    {
      ViewBag.StatusMessage =
          message == ManageMessageId.ChangePasswordSuccess ? "Uw wachtwoord is gewijzigd."
          : message == ManageMessageId.Error ? "An error has occurred."
          : "";

      var userId = User.Identity.GetUserId();
      var model = new Models.IndexViewModel
      {
        Logins = await UserManager.GetLoginsAsync(userId),
        BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
      };
      return View(model);
    }

    public virtual ActionResult ChangeName()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<ActionResult> ChangeName(Models.ChangeNameViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      var user = UserManager.FindById(User.Identity.GetUserId());
      user.FirstName = model.NewFirstName;
      user.LastName = model.NewLastName;
      var result = await UserManager.UpdateAsync(user);

      if (result.Succeeded)
      {
        return View("Index");
      }
      AddErrors(result);
      return View(model);
    }

    // GET: /Profiel/ChangeEmail
    public virtual ActionResult ChangeEmail()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<ActionResult> ChangeEmail(Models.ChangeEmailViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      var user = await UserManager.FindByEmailAsync(model.OldEmail);
      user.Email = model.NewEmail;
      user.UserName = model.NewEmail;
      var result = await UserManager.UpdateAsync(user);

      if (result.Succeeded)
      {
        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        await UserManager.SendEmailAsync(user.Id, "Uw account bevestigen", "Bevestig uw account door op volgende link te klikken: " + callbackUrl);
        ViewBag.Link = callbackUrl;
        return View("CheckEmailPrompt");
      }
      AddErrors(result);
      return View(model);
    }

    //
    // GET: /Profiel/ChangePassword
    public virtual ActionResult ChangePassword()
    {
      return View();
    }

    //
    // POST: /Profiel/ChangePassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<ActionResult> ChangePassword(Models.ChangePasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
      if (result.Succeeded)
      {
        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        if (user != null)
        {
          await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        }
        return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
      }
      AddErrors(result);
      return View(model);
    }

    public virtual ActionResult DeleteAccount()
    {
      return View();
    }

    // POST: /Profiel/DeleteAccount
    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<ActionResult> DeleteAccount(string test)
    {
      if (ModelState.IsValid)
      {
        string id = User.Identity.GetUserId();
        if (id == null)
        {
          return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        var user = UserManager.FindById(id);
        var logins = user.Logins;
        var rolesForUser = UserManager.GetRoles(id);

        foreach (var login in logins.ToList())
        {
          await _userManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
        }

        if (rolesForUser.Count() > 0)
        {
          foreach (var item in rolesForUser.ToList())
          {
            // item should be the name of the role
            var result = await _userManager.RemoveFromRoleAsync(user.Id, item);
          }
        }

        UserManager.Delete(user);

        return RedirectToAction("AccountDeleted");
      }
      else
      {
        return View();
      }
    }

    public virtual ActionResult AccountDeleted()
    {
      SignInManager.AuthenticationManager.SignOut();
      return View();
    }

    #region Helpers
    // Used for XSRF protection when adding external logins
    private const string XsrfKey = "XsrfId";

    private IAuthenticationManager AuthenticationManager
    {
      get
      {
        return HttpContext.GetOwinContext().Authentication;
      }
    }

    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError("", error);
      }
    }

    public enum ManageMessageId
    {
      ChangePasswordSuccess,
      Error,
      ChangeEmailSuccess
    }

    #endregion
  }
}