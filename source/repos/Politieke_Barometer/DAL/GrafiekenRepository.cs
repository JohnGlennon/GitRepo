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
  public class GrafiekenRepository
  {
    private PBDbContext context;

    public GrafiekenRepository()
    {
      context = new PBDbContext();
    }

    public void CreateGrafiek(Grafiek grafiek)
    {
      context.Grafieken.Add(grafiek);
      context.SaveChanges();
    }

    public IEnumerable<Grafiek> ReadGrafieken(bool dashboard, bool items)
    {
      if (!dashboard && !items) return context.Grafieken.AsEnumerable();
      if (!dashboard && items) return context.Grafieken.Include("Items").AsEnumerable();
      if (dashboard && !items) return context.Grafieken.Include("Dashboard").AsEnumerable();
      else return context.Grafieken.Include("Dashboard").Include("Items").AsEnumerable();
    }

    public Grafiek ReadGrafiek(int id, bool dashboard, bool items)
    {
      if (!dashboard && !items) return context.Grafieken.AsEnumerable().SingleOrDefault(g => g.GrafiekId == id);
      if (!dashboard && items) return context.Grafieken.Include("Items").AsEnumerable().SingleOrDefault(g => g.GrafiekId == id);
      if (dashboard && !items) return context.Grafieken.Include("Dashboard").AsEnumerable().SingleOrDefault(g => g.GrafiekId == id);
      else return context.Grafieken.Include("Dashboard").Include("Items").AsEnumerable().SingleOrDefault(g => g.GrafiekId == id);
    }

    public void UpdateGrafiek(Grafiek grafiek)
    {
      context.Entry(grafiek).State = EntityState.Modified;
      context.SaveChanges();
    }

    public void DeleteGrafiek(Grafiek grafiek)
    {
      context.Grafieken.Remove(grafiek);
      context.SaveChanges();
    }
  }
}
