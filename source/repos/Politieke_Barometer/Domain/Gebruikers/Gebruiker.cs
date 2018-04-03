using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Gebruikers
{
  public class Gebruiker
  {
    public int GebruikerId { get; set; }

    public String Naam { get; set; }
    public String Voornaam { get; set; }
    public String Email { get; set; }
    public String Wachtwoord { get; set; }
    public Rol Rol { get; set; }

    //Foreign keys
    public Dashboard Dashboard { get; set; }
    public int DashboardId { get; set; }
    public List<Alert> Alerts { get; set; }

    public Gebruiker()
    {
      Alerts = new List<Alert>();
    }
  }
}
