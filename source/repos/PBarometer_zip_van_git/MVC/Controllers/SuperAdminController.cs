using BL;
using BL.IdentityFramework;
using Domain.Dashboards;
using Domain.Deelplatformen;
using Domain.IdentityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MVC.Models;
using MVC.Models.SuperAdmin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace MVC.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public partial class SuperAdminController : Controller
    {

        // GET: SuperAdmin
        public virtual ActionResult Index()
        {
            if (RouteData.Values["deelplatform"] == null)
            {
                ViewBag.GeenDeelplatformGeselecteerd = true;
            }
            return View();
        }

        public virtual ActionResult LaadGemonitordeItemsBeheren()
        {
            return PartialView("~/Views/Shared/AdminSuperadmin/GemonitordeItemsBeheren.cshtml");
        }

        public virtual ActionResult LaadData()
        {
            return PartialView("~/Views/Shared/AdminSuperadmin/DataImporterenEnExporteren.cshtml");
        }

        public virtual ActionResult LaadGebruikersactiviteit()
        {
            return PartialView("~/Views/Shared/AdminSuperadmin/GebruikersactiviteitMonitoren.cshtml");
        }

        public virtual ActionResult LaadLayout()
        {
            return PartialView("~/Views/Shared/AdminSuperadmin/LayoutAanpassen.cshtml");
        }

        public virtual ActionResult LaadMediabronnen()
        {
            return PartialView("~/Views/Shared/Superadmin/SocialeMediabronnenInstellen.cshtml");
        }

        public virtual ActionResult LaadGebruikersgegevens()
        {
            List<ApplicationUser> users = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.ToList();
            List<GebruikersViewModel> models = new List<GebruikersViewModel>();
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            foreach (ApplicationUser user in users)
            {
                bool ok = true;
                var roles = userManager.GetRoles(user.Id);
                foreach (string role in roles)
                {
                    if (role.Equals("SuperAdmin"))
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    models.Add(new GebruikersViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
                    });
                }
            }
            return PartialView("~/Views/Shared/Superadmin/GebruikersgegevensNakijken.cshtml", models);
        }

        [HttpGet]
        public virtual async Task<ActionResult> VerwijderGebruiker(string Email)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            List<ApplicationUser> users = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.ToList();

            ApplicationUser user = users.Find(u => u.Email.Equals(Email));
            var logins = user.Logins;
            var rolesForUser = userManager.GetRoles(user.Id);

            foreach (var login in logins.ToList())
            {
                await userManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
            }

            if (rolesForUser.Count() > 0)
            {
                foreach (var item in rolesForUser.ToList())
                {
                    // item should be the name of the role
                    var result = await userManager.RemoveFromRoleAsync(user.Id, item);
                }
            }
            userManager.Delete(user);
            return RedirectToAction("Index");
        }

        public virtual ActionResult LaadDeelplatform()
        {
            return PartialView("~/Views/Shared/Superadmin/DeelplatformAanmaken.cshtml");
        }

        public virtual ActionResult LaadGebruikers()
        {
            List<ApplicationUser> users = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.ToList();
            List<SuperAdminUserViewModel> userViewModels = new List<SuperAdminUserViewModel>();
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            foreach (ApplicationUser user in users)
            {
                bool ok = true;
                var roles = userManager.GetRoles(user.Id);
                foreach (string role in roles)
                {
                    if (role.Equals("SuperAdmin"))
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    bool isAdmin = false;
                    foreach (string role in roles)
                    {
                        if (role.Equals("Admin"))
                        {
                            isAdmin = true;
                        }
                    }
                    userViewModels.Add(new SuperAdminUserViewModel
                    {
                        IsAdmin = isAdmin,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
                    }
                      );
                }
            }
            return PartialView("~/Views/Shared/AdminSuperadmin/LaadGebruikers.cshtml", userViewModels);
        }

        [HttpGet]
        public virtual ActionResult SlaAdminOp(string email, bool setAdmin)
        {
            ApplicationRoleManager roleManager = new ApplicationRoleManager();
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            List<ApplicationUser> users = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.ToList();

            ApplicationUser user = users.Find(u => u.Email.Equals(email));
            var roles = userManager.GetRoles(user.Id);
            bool isAdmin = false;
            foreach (string role in roles)
            {
                if (role.Equals("Admin"))
                {
                    isAdmin = true;
                }
            }
            var adminRole = roleManager.FindByName("Admin");
            if (!isAdmin && setAdmin)
            {
                userManager.AddToRole(user.Id, adminRole.Name);
            }
            else if (isAdmin && !setAdmin)
            {
                userManager.RemoveFromRole(user.Id, adminRole.Name);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public virtual ActionResult DeelplatformOverzicht()
        {
            DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
            return PartialView("OverzichtDeelplatform", deelplatformenManager.GetDeelplatformen().Select(a => new DeelplatformOverzichtViewModel() { Id = a.DeelplatformId,Naam = a.Naam, URL = a.URLnaam}));
        }

        [HttpGet]
        public ActionResult MaakDeelplatform()
        {
            return PartialView();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MaakDeelplatform(MaakDeelplatformViewModel maakDeelplatformViewModel)
        {
            DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
            if (ModelState.IsValid)
            {
                
                deelplatformenManager.AddDeelplatform(new Deelplatform()
                {
                    AantalDagenHistoriek = maakDeelplatformViewModel.AantalDagenHistoriek,
                    URLnaam = maakDeelplatformViewModel.URLNaam,
                    LaatsteSynchronisatie = DateTime.Now.AddYears(-100),
                    Naam = maakDeelplatformViewModel.Naam
                });
                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                List<Deelplatform> deelplatformen = deelplatformenManager.GetDeelplatformen().ToList(); ;
                foreach (var user in userManager.Users.ToList())
                {
                    foreach (var deelplatform in deelplatformen)
                    {
                        user.Dashboards.Add(new Dashboard() { DeelplatformId = deelplatform.DeelplatformId });
                    }
                    userManager.Update(user);
                }
                ViewBag.Boodschap = "Het deelplatform is aangemaakt";
                return PartialView();
            }
            return PartialView(maakDeelplatformViewModel);
        }
        [HttpGet]
        public ActionResult EditDeelplatform(int id)
        {
            DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
            Deelplatform deelplatform = deelplatformenManager.GetDeelplatform(id);
            MaakDeelplatformViewModel maakDeelplatformViewModel = new MaakDeelplatformViewModel() {
                Id = id,
                AantalDagenHistoriek = deelplatform.AantalDagenHistoriek,
                Naam = deelplatform.Naam,
                URLNaam = deelplatform.URLnaam
            };
            return PartialView(maakDeelplatformViewModel);
        }
        [HttpPost]
        public ActionResult EditDeelplatform(MaakDeelplatformViewModel deelplatformViewModel)
        {
            if (ModelState.IsValid)
            {
                DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
                Deelplatform deelplatform = deelplatformenManager.GetDeelplatform(deelplatformViewModel.Id);
                deelplatform.AantalDagenHistoriek = deelplatformViewModel.AantalDagenHistoriek;
                deelplatform.Naam = deelplatformViewModel.Naam;
                deelplatform.URLnaam = deelplatformViewModel.URLNaam;
                deelplatformenManager.ChangeDeelplatform(deelplatform);
                ViewBag.Boodschap = "Deelplatform is aangepast";
                return PartialView();
            }
            return PartialView(deelplatformViewModel);
        }

        [HttpGet]
        public ActionResult VerwijderDeelplatform(int id)
        {
            DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
            deelplatformenManager.RemoveDeelplatform(id);
            return RedirectToAction("Index","Home");
        }
        }
    }
