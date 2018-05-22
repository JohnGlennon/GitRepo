using BL;
using BL.IdentityFramework;
using Domain.Dashboards;
using Domain.Deelplatformen;
using Domain.IdentityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using MVC.App_Start;
using MVC.Models.Android;
using MVC.Models.Android.Notificaties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using static MVC.Controllers.AccountController;

namespace MVC.Controllers.Api
{
    public class AndroidController : ApiController
    {


        [Route("api/Deelplatformen")]
        public IHttpActionResult GetDeelplatformen()
        {
            DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
            List<Deelplatform> deelplatformen = deelplatformenManager.GetDeelplatformen().ToList();
            if (deelplatformen == null || deelplatformen.Count() == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                List<DeelplatformDTO> deelplatformDTOs = new List<DeelplatformDTO>();
                foreach (var deelplatform in deelplatformen)
                {
                    deelplatformDTOs.Add(new DeelplatformDTO() { Naam = deelplatform.Naam, Id = deelplatform.DeelplatformId, Afbeelding = deelplatform.AfbeeldingPad });
                }
                return Ok(deelplatformDTOs);
            }
        }

        //[Route("api/DeelplatformenAfbeelding")]
        //public IHttpActionResult GetDeelplatformAfbeelding(int deelplatformId)
        //{
        //    DeelplatformenManager deelplatformenManager = new DeelplatformenManager();
        //    Deelplatform deelplatform = deelplatformenManager.GetDeelplatform(deelplatformId);
        //    if (deelplatform == null)
        //    {
        //        return StatusCode(HttpStatusCode.NoContent);
        //    }
        //    else
        //    {
        //        return Ok(deelplatform.Afbeelding);
        //    }
        //}


        [Authorize]
        [Route("api/Grafieken")]
        public IHttpActionResult GetGrafieken(int deelplatformId)
        {
            DashboardsManager dashboardsManager = new DashboardsManager();
            Dashboard dashboard = dashboardsManager.GetDashboardVanGebruikerMetGrafieken(User.Identity.GetUserId(), deelplatformId);
            GrafiekenManager grafiekenManager = new GrafiekenManager();
            List<Grafiek> grafieken = grafiekenManager.GetGrafieken(dashboard.DashboardId, deelplatformId).ToList();
            List<GrafiekDTO> grafiekDTOs = new List<GrafiekDTO>();
            if (grafieken == null || grafieken.Count() == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                foreach (var grafiek in grafieken)
                {
                    List<string> xlabels = new List<string>();
                    List<string> legende = new List<string>();
                    foreach (var item in grafiek.XLabels)
                    {
                        xlabels.Add(item.ToString());
                    }
                    foreach (var item in grafiek.LegendeLijst)
                    {
                        legende.Add(item.ToString());
                    }
                    grafiekDTOs.Add(new GrafiekDTO()
                    {
                        GrafiekId = grafiek.GrafiekId,
                        LegendeLijst = legende,
                        Periode = grafiek.Periode,
                        Titel = grafiek.Titel,
                        ToonLegende = grafiek.ToonLegende,
                        ToonXAs = grafiek.ToonXAs,
                        ToonYAs = grafiek.ToonYAs,
                        Type = grafiek.Type.ToString(),
                        XTitel = grafiek.XTitel,
                        YTitel = grafiek.YTitel,
                        YOorsprongNul = grafiek.YOorsprongNul,
                        XLabels = xlabels,
                        XOorsprongNul = grafiek.XOorsprongNul,
                        Data = grafiek.Datawaarden
                    });
                }
                return Ok(grafiekDTOs);
            }
        }


        [Authorize]
        [Route("api/Alerts")]
        public IHttpActionResult GetAlerts()
        {
            AlertManager alertManager = new AlertManager();
            List<Alert> alerts = alertManager.GetMobieleAlerts(User.Identity.GetUserId(), true, true).ToList();
            List<AlertDTO> alertDTOs = new List<AlertDTO>();

            foreach (var alert in alerts)
            {
                alertDTOs.Add(new AlertDTO()
                {
                    Beschrijving = alert.Beschrijving,
                    Id = alert.AlertId,
                    Onderwerp = alert.GemonitordItem.Naam,
                    Triggered = alert.Triggered,
                    Geactiveerd = alert.Geactiveerd
                });
            }
            if (alerts == null || alerts.Count() == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return Ok(alertDTOs);
            }
        }

        [Authorize]
        [Route("api/StuurDeviceID")]
        [HttpGet]
        public void StuurDeviceID(string deviceID)
        {
            ApplicationUserManager userManager = new ApplicationUserManager();
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            user.DeviceID = deviceID;
            userManager.Update(user);
        }

        internal void StuurMobieleAlerts(Deelplatform deelplatform)
        {
            AlertManager alertManager = new AlertManager();
            string[] deviceIDs = alertManager.GetGetriggerdeMobieleAlerts(deelplatform.DeelplatformId).ToList().Select(a => a.Gebruiker.DeviceID).ToArray();

            var messageInformation = new Message()
            {
                notification = new Notification()
                {
                    title = deelplatform.Naam,
                    text = "U heeft een alert.",
                },

                registration_ids = deviceIDs,
                data = null
            };
            string jsonMessage = JsonConvert.SerializeObject(messageInformation);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
            request.Headers.TryAddWithoutValidation("Authorization", "key=" +
                "AAAAn3EuElM:APA91bHiwfLkLC6Eqvk3cRQDPjCY5oIn0BcqIHNrX7kBjdOqBiZNUkLG2fonkHQTBoOr2wuA_sUzxKh2prRKIQPIkGfsJ8R4PuoTThCN9l90vNANQFJIF_SjimzEaDIcyAqSxPl43Pi_");
            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = client.SendAsync(request).Result;
            }
        }
    }

}