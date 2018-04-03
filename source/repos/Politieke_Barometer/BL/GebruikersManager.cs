using DAL;
using Domain.Gebruikers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
  public class GebruikersManager
  {
    private readonly GebruikersRepository repository;

    public GebruikersManager()
    {
      repository = new GebruikersRepository();
    }

    public void AddGebruiker(Gebruiker gebruiker)
    {
      repository.CreateGebruiker(gebruiker);
    }

    public IEnumerable<Gebruiker> GetGebruikers(bool alerts = false, bool dashboard = false)
    {
      return repository.ReadGebruikers(alerts, dashboard);
    }

    public Gebruiker GetGebruiker(int id, bool alerts = false, bool dashboard = false)
    {
      return repository.ReadGebruiker(id, alerts, dashboard);
    }

    public void ChangeGebruiker(Gebruiker gebruiker)
    {
      repository.UpdateGebruiker(gebruiker);
    }

    public void RemoveGebruiker(Gebruiker gebruiker)
    {
      repository.DeleteGebruiker(gebruiker);
    }
  }
}
