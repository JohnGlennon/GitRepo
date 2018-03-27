using Domain.Gebruikers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
  public class Dashboard
  {
    public int DashboardId { get; set; }

    public int GrafsPerPagina { get; set; }

    //Foreign keys
    [Required]
    public Gebruiker Gebruiker { get; set; }
    public int GebruikerId { get; set; }
    public List<Grafiek> Grafieken { get; set; }

    public Dashboard()
    {
      Grafieken = new List<Grafiek>();
    }
  }
}
