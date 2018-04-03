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
  public class DashboardsRepository
  {
    private readonly PBDbContext context;

    public DashboardsRepository()
    {
      context = new PBDbContext();
    }

    public void CreateDashboard(Dashboard dashboard)
    {
      context.Dashboards.Add(dashboard);
      context.SaveChanges();
    }

    public IEnumerable<Dashboard> ReadDashboards(bool gebruiker, bool grafieken)
    {
      if (!gebruiker && !grafieken) return context.Dashboards.AsEnumerable();
      if (!gebruiker && grafieken) return context.Dashboards.Include("Grafieken").AsEnumerable();
      if (gebruiker && !grafieken) return context.Dashboards.Include("Gebruiker").AsEnumerable();
      else return context.Dashboards.Include("Gebruiker").Include("Grafieken").AsEnumerable();
    }

    public Dashboard ReadDashboard(int id, bool gebruiker, bool grafieken)
    {
      if (!gebruiker && !grafieken) return context.Dashboards.AsEnumerable().SingleOrDefault(d => d.DashboardId == id);
      if (!gebruiker && grafieken) return context.Dashboards.Include("Grafieken").AsEnumerable().SingleOrDefault(d => d.DashboardId == id);
      if (gebruiker && !grafieken) return context.Dashboards.Include("Gebruiker").AsEnumerable().SingleOrDefault(d => d.DashboardId == id);
      else return context.Dashboards.Include("Gebruiker").Include("Grafieken").AsEnumerable().SingleOrDefault(d => d.DashboardId == id);
    }

    public void UpdateDashboard(Dashboard dashboard)
    {
      context.Entry(dashboard).State = EntityState.Modified;
      context.SaveChanges();
    }

    public void DeleteDashboard(Dashboard dashboard)
    {
      context.Dashboards.Remove(dashboard);
      context.SaveChanges();
    }
  }
}
