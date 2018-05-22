using BL;
using BL.IdentityFramework;
using Domain.Dashboards;
using Domain.Deelplatformen;
using Domain.Gemonitordeitems;
using Domain.IdentityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MVC.Models.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public partial class AlertsController : Controller
    {
        private AlertManager alertManager = new AlertManager();
        private GemonitordeItemsManager gemonitordeItemsManager = new GemonitordeItemsManager();
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

        }
        public Deelplatform HuidigDeelplatform
        {
            get
            {
                return new DeelplatformenManager().GetDeelplatformByURL(RouteData.Values["deelplatform"].ToString());
            }
        }
        //GET: Alerts
        public virtual ActionResult Index()
        {
            if (HuidigDeelplatform == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Alert> alerts = alertManager.GetAlerts(User.Identity.GetUserId(), true, true).ToList();
            if (alerts.Count > 0)
            {
                List<AlertViewModel> alertViewModels = new List<AlertViewModel>();

                foreach (var alert in alerts)
                {
                    alertViewModels.Add(new AlertViewModel()
                    {
                        Id = alert.AlertId,
                        Beschrijving = alert.Beschrijving,
                        Mail = alert.Mail,
                        Geactiveerd = alert.Geactiveerd,
                        Mobiel = alert.Mobiel,
                        Triggered = alert.Triggered,
                        Onderwerp = alert.GemonitordItem.Naam,
                        Eenvoudig = alert.EenvoudigeAlert,
                        TriggerRedenen = alert.TriggerRedenen
                    });
                }
                return View(alertViewModels);
            }
            return View("LegeIndex");
        }

        //GET: CreateSimpleAlert
        [HttpGet]
        public virtual ActionResult CreateSimpleAlert()
        {
            ViewBag.Eigenschappen = new List<string>() { "Polariteit", "Objectiviteit", "Aantal Vermeldingen" }.Select(x => new SelectListItem() { Text = x, Value = x });
            ViewBag.Trend = new List<string>() { "Stijgend", "Dalend", "Neutraal" }.Select(x => new SelectListItem() { Text = x, Value = x });
            var user = UserManager.FindById(User.Identity.GetUserId());
            List<string> items = gemonitordeItemsManager.GetGemonitordeItems(HuidigDeelplatform.DeelplatformId).ToList().OrderBy(a => a.Naam).Select(a => a.Naam).ToList();
            var ItemsSelectlist = items.Select(x => new SelectListItem() { Text = x, Value = x });

            ViewBag.Onderwerp = ItemsSelectlist;

            return PartialView();
        }

        //POST: CreateSimpleAlert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult CreateSimpleAlert(CreateBasicAlertViewModel createBasicAlertViewModel)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            GemonitordItem gemonitordItem = gemonitordeItemsManager.GetGemonitordItem(HuidigDeelplatform.DeelplatformId, createBasicAlertViewModel.Onderwerp);

            Alert alert = new Alert()
            {
                Beschrijving = createBasicAlertViewModel.Beschrijving,
                GemonitordItemId = gemonitordItem.GemonitordItemId,
                Mail = createBasicAlertViewModel.Mail,
                Mobiel = createBasicAlertViewModel.Mobiel,
                Geactiveerd = true,
                EenvoudigeAlert = true,
                DeelplatformId = HuidigDeelplatform.DeelplatformId,
            };
            Trend trend;
            switch (createBasicAlertViewModel.Trend)
            {
                case "Stijgend": trend = Trend.UP; break;
                case "Dalend": trend = Trend.DOWN; break;
                default: trend = Trend.DOWN; break;
            }
            switch (createBasicAlertViewModel.Eigenschap)
            {
                case "Polariteit":
                    alert.PolariteitsTrend = trend;
                    break;
                case "Objectiviteit":
                    alert.ObjectiviteitsTrend = trend;
                    break;
                case "Aantal Vermeldingen":
                    alert.VermeldingenTrend = trend;
                    break;
            }

            if (user.Alerts == null)
            {
                user.Alerts = new List<Alert>();
            }
            user.Alerts.Add(alert);
            UserManager.Update(user);
            return RedirectToAction("Index");
        }

        //GET: Edit
        [HttpGet]
        public virtual ActionResult EditComplexeAlert(int id)
        {
            Alert alert = alertManager.GetAlert(id);
            CreateAlertViewModel createAlertViewModel = new CreateAlertViewModel()
            {
                Id = id,
                Beschrijving = alert.Beschrijving,
                BelangrijkheidsPeriode = alert.BelangrijkheidsPeriode,
                BelangrijkWaarde = alert.BelangrijkWaarde,
                NietBelangrijkWaarde = alert.NietBelangrijkWaarde,
                Mail = alert.Mail,
                Mobiel = alert.Mobiel,
                MaxObjectiviteit = alert.MaxObjectiviteit,
                MinObjectiviteit = alert.MinObjectiviteit,
                ObjectiviteitsPeriode = alert.ObjectiviteitsPeriode,
                MaxPolariteit = alert.MaxPolariteit,
                MinPolariteit = alert.MinPolariteit,
                PolariteitsPeriode = alert.PolariteitsPeriode,
                MinDaling = alert.MinDaling,
                MinDalingPeriode = alert.MinDalingPeriode,
                MinStijging = alert.MinStijging,
                MinStijgingPeriode = alert.MinStijging,
            };

            List<string> items = gemonitordeItemsManager.GetGemonitordeItems(HuidigDeelplatform.DeelplatformId).ToList().OrderBy(a => a.Naam).Select(a => a.Naam).ToList();
            var ItemsSelectlist = items.Select(x => new SelectListItem() { Text = x, Value = x });

            ViewBag.Onderwerp = ItemsSelectlist;

            return PartialView("EditComplexeAlert", createAlertViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult EditComplexeAlert(CreateAlertViewModel alertViewModel)
        {
            Alert alert = alertManager.GetAlert(alertViewModel.Id);

            GemonitordItem gemonitordItem = gemonitordeItemsManager.GetGemonitordItem(HuidigDeelplatform.DeelplatformId, alertViewModel.Onderwerp);

            alert.Beschrijving = alertViewModel.Beschrijving;
            alert.EenvoudigeAlert = false;
            alert.Geactiveerd = true;
            alert.BelangrijkheidsPeriode = alertViewModel.BelangrijkheidsPeriode;
            alert.BelangrijkWaarde = alertViewModel.BelangrijkWaarde;
            alert.NietBelangrijkWaarde = alertViewModel.NietBelangrijkWaarde;
            alert.Mail = alertViewModel.Mail;
            alert.Mobiel = alertViewModel.Mobiel;
            alert.MaxObjectiviteit = alertViewModel.MaxObjectiviteit;
            alert.MinObjectiviteit = alertViewModel.MinObjectiviteit;
            alert.ObjectiviteitsPeriode = alertViewModel.ObjectiviteitsPeriode;
            alert.MaxPolariteit = alertViewModel.MaxPolariteit;
            alert.MinPolariteit = alertViewModel.MinPolariteit;
            alert.PolariteitsPeriode = alertViewModel.PolariteitsPeriode;
            alert.MinDaling = alertViewModel.MinDaling;
            alert.MinDalingPeriode = alertViewModel.MinDalingPeriode;
            alert.MinStijging = alertViewModel.MinStijging;
            alert.MinStijgingPeriode = alertViewModel.MinStijging;
            alert.GemonitordItemId = gemonitordItem.GemonitordItemId;
            alert.Triggered = false;
            alertManager.ChangeAlert(alert);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public virtual ActionResult EditEenvoudigeAlert(int id)
        {
            Alert alert = alertManager.GetAlert(id, gemonitordItem: true);

            ViewBag.Eigenschappen = new List<string>() { "Polariteit", "Objectiviteit", "Aantal Vermeldingen" }.Select(x => new SelectListItem() { Text = x, Value = x });
            ViewBag.Trend = new List<string>() { "Stijgend", "Dalend", "Neutraal" }.Select(x => new SelectListItem() { Text = x, Value = x });
            List<string> items = gemonitordeItemsManager.GetGemonitordeItems(HuidigDeelplatform.DeelplatformId).ToList().OrderBy(a => a.Naam).Select(a => a.Naam).ToList();
            var ItemsSelectlist = items.Select(x => new SelectListItem() { Text = x, Value = x });

            ViewBag.Onderwerp = ItemsSelectlist;
            CreateBasicAlertViewModel alertViewModel = new CreateBasicAlertViewModel()
            {
                Id = id,
                Beschrijving = alert.Beschrijving,
                Mail = alert.Mail,
                Mobiel = alert.Mobiel,
                Onderwerp = alert.GemonitordItem.Naam,
            };
            return PartialView("EditEenvoudigeAlert", alertViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult EditEenvoudigeAlert(CreateBasicAlertViewModel alertViewModel)
        {
            Alert alert = alertManager.GetAlert(alertViewModel.Id);
            GemonitordItem gemonitordItem = gemonitordeItemsManager.GetGemonitordItem(1, alertViewModel.Onderwerp);
            alert.Beschrijving = alertViewModel.Beschrijving;
            alert.GemonitordItemId = gemonitordItem.GemonitordItemId;
            alert.Mail = alertViewModel.Mail;
            alert.Mobiel = alertViewModel.Mobiel;
            alert.Triggered = false;
            Trend trend;
            switch (alertViewModel.Trend)
            {
                case "Stijgend": trend = Trend.UP; break;
                case "Dalend": trend = Trend.DOWN; break;
                default: trend = Trend.DOWN; break;
            }
            switch (alertViewModel.Eigenschap)
            {
                case "Polariteit":
                    alert.PolariteitsTrend = trend;
                    break;
                case "Objectiviteit":
                    alert.ObjectiviteitsTrend = trend;
                    break;
                case "Aantal Vermeldingen":
                    alert.VermeldingenTrend = trend;
                    break;
            }
            alertManager.ChangeAlert(alert);
            return RedirectToAction("Index");
        }

        //GET: CreateAlert
        [HttpGet]
        public virtual ActionResult CreateAlert()
        {
            List<string> items = gemonitordeItemsManager.GetGemonitordeItems(1).ToList().OrderBy(a => a.Naam).Select(a => a.Naam).ToList();
            var ItemsSelectlist = items.Select(x => new SelectListItem() { Text = x, Value = x });

            ViewBag.Onderwerp = ItemsSelectlist;

            return PartialView();
        }
        //POST: CreateAlert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult CreateAlert(CreateAlertViewModel alertViewModel)
        {
            if (ModelState.IsValid)
            {
                GemonitordItem gemonitordItem = gemonitordeItemsManager.GetGemonitordItem(1, alertViewModel.Onderwerp);
                Alert alert = new Alert()
                {
                    Beschrijving = alertViewModel.Beschrijving,
                    EenvoudigeAlert = false,
                    Geactiveerd = true,
                    BelangrijkheidsPeriode = alertViewModel.BelangrijkheidsPeriode,
                    BelangrijkWaarde = alertViewModel.BelangrijkWaarde,
                    NietBelangrijkWaarde = alertViewModel.NietBelangrijkWaarde,
                    Mail = alertViewModel.Mail,
                    Mobiel = alertViewModel.Mobiel,
                    MaxObjectiviteit = alertViewModel.MaxObjectiviteit,
                    MinObjectiviteit = alertViewModel.MinObjectiviteit,
                    ObjectiviteitsPeriode = alertViewModel.ObjectiviteitsPeriode,
                    MaxPolariteit = alertViewModel.MaxPolariteit,
                    MinPolariteit = alertViewModel.MinPolariteit,
                    PolariteitsPeriode = alertViewModel.PolariteitsPeriode,
                    MinDaling = alertViewModel.MinDaling,
                    MinDalingPeriode = alertViewModel.MinDalingPeriode,
                    MinStijging = alertViewModel.MinStijging,
                    MinStijgingPeriode = alertViewModel.MinStijging,
                    GemonitordItemId = gemonitordItem.GemonitordItemId,
                    DeelplatformId = HuidigDeelplatform.DeelplatformId
                };
                var user = UserManager.FindById(User.Identity.GetUserId());
                if (user.Alerts == null)
                {
                    user.Alerts = new List<Alert>();
                }
                user.Alerts.Add(alert);
                UserManager.Update(user);
                return RedirectToAction("Index");
            }
            List<string> items = gemonitordeItemsManager.GetGemonitordeItems(HuidigDeelplatform.DeelplatformId).ToList().OrderBy(a => a.Naam).Select(a => a.Naam).ToList();
            var ItemsSelectlist = items.Select(x => new SelectListItem() { Text = x, Value = x });

            ViewBag.Onderwerp = ItemsSelectlist;
            return RedirectToAction("Index");
        }
        [HttpGet]
        public virtual ActionResult VerwijderAlert(int id)
        {
            alertManager.RemoveAlert(alertManager.GetAlert(id));
            return RedirectToAction("Index");
        }
        [HttpGet]
        public virtual ActionResult ToggleActivatie(int id)
        {
            Alert alert = alertManager.GetAlert(id);
            alert.Geactiveerd = !alert.Geactiveerd;
            alertManager.ChangeAlert(alert);

            return RedirectToAction("Index");
        }

        public virtual ActionResult Gelezen(int id)
        {
            Alert alert = alertManager.GetAlert(id);
            alert.Triggered = false;
            alertManager.ChangeAlert(alert);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult IsTriggered()
        {
            bool triggered = false;
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            foreach (Alert alert in user.Alerts)
            {
                if (alert.Triggered)
                {
                    //Break omdat 1 getriggerede alert genoeg is
                    triggered = true;
                    break;
                }
            }
            return Json(new { status = triggered });
        }
    }
}