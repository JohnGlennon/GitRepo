using DAL.EF;
using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AlertRepository
    {
        private EF.DbContext context;

        public AlertRepository()
        {
            context = new EF.DbContext();
        }

        public AlertRepository(UnitOfWork uow)
        {
            context = uow.Context;
        }

        public void CreateAlert(Alert alert)
        {
            context.Alerts.Add(alert);
            context.SaveChanges();
        }

        public IEnumerable<Alert> ReadAlerts(bool gebruiker, bool gemonitordItem)
        {
            if (!gebruiker && !gemonitordItem) return context.Alerts.AsEnumerable();
            if (!gebruiker && gemonitordItem) return context.Alerts.Include("GemonitordItem").AsEnumerable();
            if (gebruiker && !gemonitordItem) return context.Alerts.Include("Gebruiker").AsEnumerable();
            else return context.Alerts.Include("Gebruiker").Include("GemonitordItem").AsEnumerable();
        }

        public Alert ReadAlert(int id, bool gebruiker, bool gemonitordItem)
        {
            if (!gebruiker && !gemonitordItem) return context.Alerts.AsEnumerable().SingleOrDefault(a => a.AlertId == id);
            if (!gebruiker && gemonitordItem) return context.Alerts.Include("GemonitordItem").AsEnumerable().SingleOrDefault(a => a.AlertId == id);
            if (gebruiker && !gemonitordItem) return context.Alerts.Include("Gebruiker").AsEnumerable().SingleOrDefault(a => a.AlertId == id);
            else return context.Alerts.Include("Gebruiker").Include("GemonitordItem").AsEnumerable().SingleOrDefault(a => a.AlertId == id);
        }

        public void UpdateAlert(Alert alert)
        {
            context.Entry(alert).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteAlerts(IEnumerable<Alert> alerts)
        {
            if (alerts.Count() > 0)
            {
                foreach (var alert in alerts)
                {
                    context.Alerts.Remove(alert);
                }
                context.SaveChanges();
            }

        }

        public void DeleteAlert(Alert alert)
        {
            context.Alerts.Remove(alert);
            context.SaveChanges();
        }
    }
}
