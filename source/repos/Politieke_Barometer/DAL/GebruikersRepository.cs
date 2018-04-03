using DAL.EF;
using Domain.Gebruikers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public class GebruikersRepository
  {
    private readonly PBDbContext context;

    public GebruikersRepository()
    {
      context = new PBDbContext();
    }

    public void CreateGebruiker(Gebruiker gebruiker)
    {
      context.Gebruikers.Add(gebruiker);
      context.SaveChanges();
    }

    public IEnumerable<Gebruiker> ReadGebruikers(bool alerts, bool dashboard)
    {
      if (!alerts && !dashboard) return context.Gebruikers.AsEnumerable();
      if (!alerts && dashboard) return context.Gebruikers.Include("Dashboard").AsEnumerable();
      if (alerts && !dashboard) return context.Gebruikers.Include("Alerts").AsEnumerable();
      else return context.Gebruikers.Include("Alerts").Include("Dashboard").AsEnumerable();
    }

    public Gebruiker ReadGebruiker(int id, bool alerts, bool dashboard)
    {
      if (!alerts && !dashboard) return context.Gebruikers.AsEnumerable().SingleOrDefault(g => g.GebruikerId == id);
      if (!alerts && dashboard) return context.Gebruikers.Include("Dashboard").AsEnumerable().SingleOrDefault(g => g.GebruikerId == id);
      if (alerts && !dashboard) return context.Gebruikers.Include("Alerts").AsEnumerable().SingleOrDefault(g => g.GebruikerId == id);
      else return context.Gebruikers.Include("Alerts").Include("Dashboard").AsEnumerable().SingleOrDefault(g => g.GebruikerId == id);
    }

    public void UpdateGebruiker(Gebruiker gebruiker)
    {
      context.Entry(gebruiker).State = EntityState.Modified;
      context.SaveChanges();
    }

    public void DeleteGebruiker(Gebruiker gebruiker)
    {
      context.Gebruikers.Remove(gebruiker);
      context.SaveChanges();
    }
  }
}
