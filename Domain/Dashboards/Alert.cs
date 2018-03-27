using Domain.Gebruikers;
using Domain.Gemonitordeitems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
  public class Alert
  {
    public int AlertId { get; set; }

    //Mail: De status van de alert wordt per mail naar de gebruiker gestuurd.
    //Mobiel: De status van de alert wordt op de mobiele app getoond.
    public bool Mail { get; set; }
    public bool Mobiel { get; set; }

    //MinDaling: De minimum daling in percentage die de alert moet triggeren
    //DalingDagenAantal: Het aantal dagen waarop de daling moet berekend worden
    public double MinDaling { get; set; }
    public double DalingDagenAantal { get; set; }

    //MinStijging: De minimum stijging in percentage die de alert moet triggeren
    //StijgingDagenAantal: Het aantal dagen waarop de stijging moet berekend worden
    public double MinStijging { get; set; }
    public double StijgingDagenAantal { get; set; }

    //NietBelangrijkWaarde: De waarde wanneer een item niet als relevant meer wordt geacht.
    //NietBelangrijkheidsPeriode: De periode waarop de onbelangrijkheid moet berekend worden.
    public double NietBelangrijkWaarde { get; set; }
    public double NietBelangrijkheidsPeriode { get; set; }

    //BelangrijkWaarde: De waarde wanneer een item als belangrijk wordt geacht.
    //BelangrijkHeidsPeriode: De periode waarop de belangrijkheid wordt berekend.
    public double BelangrijkWaarde { get; set; }
    public double BelangrijkheidsPeriode { get; set; }

    //MinPolariteit: De minimumpolariteit die de alert moet triggeren.
    //MaxPolariteit: De maximumpolariteit die de alert moet triggeren.
    public double MinPolariteit { get; set; }
    public double MaxPolariteit { get; set; }

    //Triggered: De status die aangeeft of de alert getriggerd werd of niet.
    public bool Triggered { get; set; }

    //Foreign keys
    [Required]
    public GemonitordItem Item { get; set; }
    public int GemonitordItemId { get; set; }
    [Required]
    public Gebruiker Gebruiker { get; set; }
    public int GebruikerId { get; set; }

    public Alert()
    {
      Triggered = false;
    }
  }
}
