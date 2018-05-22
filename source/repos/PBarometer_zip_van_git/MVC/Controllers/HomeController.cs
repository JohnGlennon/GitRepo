using System.Collections.Generic;
using System.Web.Mvc;
using BL;
using MVC.Controllers.Api;
using Domain.Dashboards;
using Domain.Gemonitordeitems;
using System.Linq;
using Domain.Deelplatformen;
using Microsoft.AspNet.Identity;
using MVC.Models.Home;

namespace MVC.Controllers
{
    public partial class HomeController : Controller
    {


        public virtual ActionResult Index()
        {
            if (RouteData.Values["deelplatform"] == null)
            {
                ViewBag.GeenDeelplatformGeselecteerd = true;
            }
            DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
            List<Deelplatform> deelplatformen = deelplatformenManager.GetDeelplatformen().ToList();
            List<DeelplatformViewModel> deelplatformViewModels = new List<DeelplatformViewModel>();
            foreach (var deelplatform in deelplatformen)
            {
                string afbeeldingPad = deelplatform.AfbeeldingPad;
                if (afbeeldingPad == null)
                {
                    afbeeldingPad = "default.png";
                }
                deelplatformViewModels.Add(new DeelplatformViewModel()
                {
                    Naam = deelplatform.Naam,
                    Afbeelding = afbeeldingPad,
                    URL = deelplatform.URLnaam
                });
            }
            return View(deelplatformViewModels);
        }

        public virtual ActionResult AddItems()
        {
            DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
            int id = deelplatformenManager.GetDeelplatformByName("Politieke Barometer").DeelplatformId;
            GemonitordeItemsManager gemonitordeItemsManager = new GemonitordeItemsManager();
            gemonitordeItemsManager.AddOrganisatie("Open VLD", id, new List<string>() { "Alexander De Croo", "Gwendolyn Rutten", "Maggie De Block" });
            gemonitordeItemsManager.AddOrganisatie("Groen", id, new List<string>() { "Kristof Calvo", "Meyrem Almaci", "Wouter Van Besien" });
            gemonitordeItemsManager.AddOrganisatie("SPA", id, new List<string>() { "Caroline Gennez", "John Crombez", "Bruno Tobback" });
            gemonitordeItemsManager.AddOrganisatie("Vlaams Belang", id, new List<string>() { "Filip Dewinter", "Tom Van Grieken", "Gerolf Annemans" });

            gemonitordeItemsManager.AddThema("Migratie", new List<string>() { "buitenland", "vluchteling", "immigratie", "migratie" }, id);
            gemonitordeItemsManager.AddThema("Fiscaliteit", new List<string>() { "belastingen", "tax", "btw", "sociale zekerheid" }, id);
            gemonitordeItemsManager.AddThema("Milieu", new List<string>() { "kernenergie", "zonnenergie", "steenkool", "luchtvervuiling", "windenergie" }, id);

            List<GemonitordItem> gemonitordeItems = gemonitordeItemsManager.GetGemonitordeItems(1).ToList();
            GrafiekenManager grafiekenManager = new GrafiekenManager();

            List<GrafiekItem> grafiekItems1 = new List<GrafiekItem>()
      {
        new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(0).GemonitordItemId },
        new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(1).GemonitordItemId },
        new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(2).GemonitordItemId }
      };

            List<GrafiekWaarde> waarden1 = new List<GrafiekWaarde>()
      {
        GrafiekWaarde.Objectiviteit,
        GrafiekWaarde.Polariteit,
        GrafiekWaarde.Objectiviteit
      };

            Grafiek grafiek1 = new Grafiek()
            {
                DashboardId = 1,
                //Periode = 20,
                Titel = "Grafiek van de coole items",
                ToonLegende = true,
                ToonXAs = true,
                ToonYAs = true,
                //Keuze = GrafiekKeuze.VergelijkingItemsDoorheenDeTijd,
                //XOnder = false,
                XOorsprongNul = true,
                XTitel = "Items",
                YOorsprongNul = true,
                YTitel = "Waarden",
                //GrafiekItems = grafiekItems1,
                //Waarden = waarden1
            };

            grafiekenManager.AddGrafiek(grafiek1);

            List<GrafiekItem> grafiekItems2 = new List<GrafiekItem>()
      {
        new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(3).GemonitordItemId },
        new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(4).GemonitordItemId },
        new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(5).GemonitordItemId }
      };

            List<GrafiekWaarde> waarden2 = new List<GrafiekWaarde>()
      {
        GrafiekWaarde.Objectiviteit,
        GrafiekWaarde.Polariteit,
        GrafiekWaarde.Objectiviteit
      };

            Grafiek grafiek2 = new Grafiek()
            {
                DashboardId = 1,
                //Periode = 20,
                Titel = "Nog een grafiekje van de coole items",
                ToonLegende = true,
                ToonXAs = true,
                ToonYAs = true,
                //Keuze = GrafiekKeuze.VergelijkingItemsDoorheenDeTijd,
                //XOnder = false,
                XOorsprongNul = true,
                XTitel = "Items",
                YOorsprongNul = true,
                YTitel = "Waarden",
                //GrafiekItems = grafiekItems2,
                //Waarden = waarden2
            };

            grafiekenManager.AddGrafiek(grafiek2);

            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult AddGrafieken()
        {
            DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
            int id = deelplatformenManager.GetDeelplatformByName("Politieke Barometer").DeelplatformId;
            GemonitordeItemsManager gemonitordeItemsManager = new GemonitordeItemsManager();
            List<GemonitordItem> gemonitordeItems = gemonitordeItemsManager.GetGemonitordeItems(1).ToList();
            GrafiekenManager grafiekenManager = new GrafiekenManager();

            List<GrafiekItem> grafiekItems1 = new List<GrafiekItem>()
            {
                new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(0).GemonitordItemId },
                new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(1).GemonitordItemId },
                new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(2).GemonitordItemId }
            };

            List<GrafiekWaarde> waarden1 = new List<GrafiekWaarde>()
            {
                GrafiekWaarde.Objectiviteit,
                GrafiekWaarde.Polariteit,
                GrafiekWaarde.Objectiviteit
            };
            DashboardsManager dashboardsManager = new DashboardsManager();
            int dashboardId = dashboardsManager.GetDashboardVanGebruikerMetGrafieken(User.Identity.GetUserId(), id).DashboardId;
            Grafiek grafiek1 = new Grafiek()
            {
                DashboardId = dashboardId,
                //Periode = 20,
                Titel = "Grafiek van de coole items",
                ToonLegende = true,
                ToonXAs = true,
                ToonYAs = true,
                //Keuze = GrafiekKeuze.VergelijkingItemsDoorheenDeTijd,
                //XOnder = false,
                XOorsprongNul = true,
                XTitel = "Items",
                YOorsprongNul = true,
                YTitel = "Waarden",
                //GrafiekItems = grafiekItems1,
                //Waarden = waarden1
            };

            grafiekenManager.AddGrafiek(grafiek1);

            List<GrafiekItem> grafiekItems2 = new List<GrafiekItem>()
            {
                new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(3).GemonitordItemId },
                new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(4).GemonitordItemId },
                new GrafiekItem(){ ItemId = gemonitordeItems.ElementAt(5).GemonitordItemId }
            };

            List<GrafiekWaarde> waarden2 = new List<GrafiekWaarde>()
            {
                GrafiekWaarde.Objectiviteit,
                GrafiekWaarde.Polariteit,
                GrafiekWaarde.Objectiviteit
            };

            Grafiek grafiek2 = new Grafiek()
            {
                DashboardId = dashboardId,
                //Periode = 20,
                Titel = "Nog een grafiekje van de coole items",
                ToonLegende = true,
                ToonXAs = true,
                ToonYAs = true,
                //Keuze = GrafiekKeuze.VergelijkingItemsDoorheenDeTijd,
                //XOnder = false,
                XOorsprongNul = true,
                XTitel = "Items",
                YOorsprongNul = true,
                YTitel = "Waarden",
                //GrafiekItems = grafiekItems2,
                //Waarden = waarden2
            };

            grafiekenManager.AddGrafiek(grafiek2);

            return RedirectToAction("Index");
        }

        public virtual ActionResult GetData()
        {
            DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
            TextgainController textgainController = new TextgainController();
            textgainController.HaalBerichtenOp(deelplatformenManager.GetDeelplatformByName("Politieke Barometer"));

            return RedirectToAction("Index");
        }
    }
}