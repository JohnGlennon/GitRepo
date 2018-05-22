using BL.IdentityFramework;
using Domain.IdentityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using reCAPTCHA.MVC;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using MVC.Models;
using Domain.Dashboards;
using Domain.Deelplatformen;
using System.Collections.Generic;
using BL;

namespace MVC.Controllers
{
  public partial class AccountController : Controller
  {
    public AccountController()
    {
    }

    public ApplicationUserManager UserManager
    {
      get
      {
        return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
      }

    }


    public ApplicationSignInManager SignInManager
    {
      get
      {
        return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
      }

    }

    //
    // GET: /Account/Login
    [AllowAnonymous]
    public virtual ActionResult Login(string returnUrl)
    {
      ViewBag.ReturnUrl = returnUrl;
      return View();
    }

    //
    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public virtual async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      // This doesn't count login failures towards account lockout
      // To enable password failures to trigger account lockout, change to shouldLockout: true
      var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
      switch (result)
      {
        case SignInStatus.Success:
          return RedirectToLocal(returnUrl);
        case SignInStatus.LockedOut:
          return View("Lockout");
        case SignInStatus.RequiresVerification:
          return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        case SignInStatus.Failure:
        default:
          ModelState.AddModelError("", "Ongeldige inlogpoging.");
          return View(model);
      }
    }

    //
    // GET: /Account/Register
    [AllowAnonymous]
    public virtual ActionResult Register()
    {
      return View();
    }

    //
    // POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [CaptchaValidator(
      PrivateKey = "6LfotVAUAAAAAI8plTIiw3g2g8jy311qRtkgMaEp",
      ErrorMessage = "U bent een robot.",
      RequiredMessage = "De captcha moet ingevuld worden.")]
    public virtual async Task<ActionResult> Register(RegisterViewModel model)
    {
      if (model.Email != null)
      {
        var user = UserManager.FindByEmail(model.Email);
        if (ModelState.IsValid && user == null && model != null)
        {
          var newUser = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
          DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
          foreach (var deelplatform in deelplatformenManager.GetDeelplatformen())
          {
            newUser.Dashboards.Add(new Dashboard() { DeelplatformId = deelplatform.DeelplatformId });
          }
          var userResult = await UserManager.CreateAsync(newUser, model.Password);
          if (userResult.Succeeded)
          {
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(newUser.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(newUser.Id, "Uw account bevestigen", "Bevestig uw account door op volgende link te klikken: " + callbackUrl);
            ViewBag.Link = callbackUrl;
            return View("CheckEmailPrompt");
          }
          AddErrors(userResult);
        }
        if (user != null)
        {
          ModelState.AddModelError("", "Er is al een gebruiker met dit e-mailadres geregistreerd");
        }
      }
      // If we got this far, something failed, redisplay form
      return View(model);
    }

    //
    // GET: /Account/ConfirmEmail
    [AllowAnonymous]
    public virtual async Task<ActionResult> ConfirmEmail(string userId, string code)
    {
      if (userId == null || code == null)
      {
        return View("Error");
      }
      var result = await UserManager.ConfirmEmailAsync(userId, code);
      return View(result.Succeeded ? "ConfirmEmail" : "Error");
    }

    //
    // GET: /Account/ForgotPassword
    [AllowAnonymous]
    public virtual ActionResult ForgotPassword()
    {
      return View();
    }

    //
    // POST: /Account/ForgotPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public virtual async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
      if (ModelState.IsValid)
      {
        var user = await UserManager.FindByNameAsync(model.Email);
        if (user != null)
        {
          var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
          var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
          await UserManager.SendEmailAsync(user.Id, "Reset Password", "Reset uw wachtwoord door op volgende link te klikken:  " + callbackUrl);
        }
        return View("ForgotPasswordConfirmed");
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    //
    // GET: /Account/ForgotPasswordConfirmation
    [AllowAnonymous]
    public virtual ActionResult ForgotPasswordConfirmation()
    {
      return View();
    }

    //
    // GET: /Account/ResetPassword
    [AllowAnonymous]
    public virtual ActionResult ResetPassword(string code)
    {
      return code == null ? View("Error") : View();
    }

    //
    // POST: /Account/ResetPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      var user = await UserManager.FindByNameAsync(model.Email);
      if (user == null)
      {
        // Don't reveal that the user does not exist
        return RedirectToAction("ResetPasswordConfirmation", "Account");
      }
      var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
      if (result.Succeeded)
      {
        return RedirectToAction("ResetPasswordConfirmation", "Account");
      }
      AddErrors(result);
      return View();
    }

    //
    // GET: /Account/ResetPasswordConfirmation
    [AllowAnonymous]
    public virtual ActionResult ResetPasswordConfirmation()
    {
      return View();
    }

    // POST: /Account/ExternalLogin
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public void ExternalLogin(string provider, string returnUrl)
    {
      // Request a redirect to the external login provider
      AuthenticationProperties properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }) };
      HttpContext.GetOwinContext().Authentication.Challenge(properties, provider);
    }

    // POST: /Account/ExternalLoginConfirmation
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public virtual async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
    {
      if (User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Index", "Home");
      }

      if (ModelState.IsValid)
      {
        // Get the information about the user from the external login provider
        var info = await HttpContext.GetOwinContext().Authentication.GetExternalLoginInfoAsync();
        if (info == null)
        {
          return View("Error");
        }
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await UserManager.CreateAsync(user);
        if (result.Succeeded)
        {
          result = await UserManager.AddLoginAsync(user.Id, info.Login);
          if (result.Succeeded)
          {
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            return RedirectToAction("Index", "Home");
          }
          else
          {
            return View("Error");
          }
        }

      }
      ViewBag.ReturnUrl = returnUrl;
      return View(model);
    }

    //
    // GET: /Account/ExternalLoginCallback
    [AllowAnonymous]
    public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl)
    {
      var loginInfo = await HttpContext.GetOwinContext().Authentication.GetExternalLoginInfoAsync();
      if (loginInfo == null)
      {
        return RedirectToAction("Login");
      }

      // Sign in the user with this external login provider if the user already has a login
      var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
      switch (result)
      {
        case SignInStatus.Success:
          return RedirectToAction("Index", "Home");
        case SignInStatus.LockedOut:
          return View("Lockout");
        case SignInStatus.RequiresVerification:
          return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
        case SignInStatus.Failure:
        default:
          // If the user does not have an account, then prompt the user to create an account
          ViewBag.ReturnUrl = returnUrl;
          ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
          return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
      }
    }

    // POST: /Account/LogOff
    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual ActionResult LogOff()
    {
      AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
      return RedirectToAction("Index", "Home");
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

    private ActionResult RedirectToLocal(string returnUrl)
    {
      if (Url.IsLocalUrl(returnUrl))
      {
        return Redirect(returnUrl);
      }
      return RedirectToAction("Index", "Home");
    }

    internal class ChallengeResult : HttpUnauthorizedResult
    {
      public ChallengeResult(string provider, string redirectUri)
          : this(provider, redirectUri, null)
      {
      }

      public ChallengeResult(string provider, string redirectUri, string userId)
      {
        LoginProvider = provider;
        RedirectUri = redirectUri;
        UserId = userId;
      }

      public string LoginProvider { get; set; }
      public string RedirectUri { get; set; }
      public string UserId { get; set; }

      public override void ExecuteResult(ControllerContext context)
      {
        var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
        if (UserId != null)
        {
          properties.Dictionary[XsrfKey] = UserId;
        }
        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
      }
    }
    #endregion
  }
}