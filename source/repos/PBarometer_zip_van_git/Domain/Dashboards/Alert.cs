using Domain.Deelplatformen;
using Domain.Gemonitordeitems;
using Domain.IdentityFramework;
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
    //AlertId: Id van een Alert.
    public int AlertId { get; set; }

    //Mail: De gebruiker kiest of de Alert via mail wordt gestuurd.
    //Mobiel: De gebruiker kiest of de Alert via de mobiele applicatie wordt gestuurd.
    public bool Mail { get; set; }
    public bool Mobiel { get; set; }

    //Geactiveerd: De gebruiker kan een Alert deactiveren zonder de Alert zelf te verwijderen.
    //Zo moet de gebruiker de Alert niet elke keer verwijderen als het mogelijk zou zijn dat
    //deze later nog gebruikt zal worden.
    public bool Geactiveerd { get; set; }

    //MinDaling: De minimum daling in percentage vergeleken met de vorige periode die de Alert moet triggeren.
    //MinDalingPeriode: Het aantal dagen waarop de daling moet berekend worden.
    public double MinDaling { get; set; }
    public double MinDalingPeriode { get; set; }

    //MinStijging: De minimum stijging in percentage vergeleken met de vorige periode die de Alert moet triggeren.
    //MinStijgingPeriode: Het aantal dagen waarop de stijging moet berekend worden.
    public double MinStijging { get; set; }
    public double MinStijgingPeriode { get; set; }

    //NietBelangrijkWaarde: Het aantal vermeldingen gedurende een periode in verband met een GemonitordItem dat niet als relevant meer wordt geacht.
    //BelangrijkWaarde: Het aantal vermeldingen gedurende een periode in verband met een GemonitordItem dat als belangrijk wordt geacht.
    //BelangrijkHeidsPeriode: Het aantal dagen waarop de belangrijkheid of niet-belangrijkheid wordt berekend.
    public double NietBelangrijkWaarde { get; set; }
    public double BelangrijkWaarde { get; set; }
    public double BelangrijkheidsPeriode { get; set; }

    //MinPolariteit: De minimumpolariteit die de Alert moet triggeren.
    //MaxPolariteit: De maximumpolariteit die de Alert moet triggeren.
    //PolariteitsPeriode: Het aantal dagen waarop de gemiddelde polariteit wordt berekend.
    public double MinPolariteit { get; set; }
    public double MaxPolariteit { get; set; }
    public double PolariteitsPeriode { get; set; }

    //MinObjectiviteit: De minimumobjectiviteit die de Alert moet triggeren.
    //MaxObjectiviteit: De maximumobjectiviteit die de Alert moet triggeren.
    //Objectiviteitsperiode: Het aantal dagen waarop de gemiddelde objectiviteit wordt berekend.
    public double MinObjectiviteit { get; set; }
    public double MaxObjectiviteit { get; set; }
    public double ObjectiviteitsPeriode { get; set; }

    //Triggered: De status die aangeeft of de Alert getriggerd werd of niet.
    public bool Triggered { get; set; }
    //Trends: De trends van het gemonitorditem
    public Trend? PolariteitsTrend { get; set; }
    public Trend? ObjectiviteitsTrend { get; set; }
    public Trend? VermeldingenTrend { get; set; }
    //TriggerRedenen: De opsomming waarom de Alert getriggerd werd.
    public string TriggerRedenen { get; set; }
    //Beschrijving: Beschrijving van de alert
    public string Beschrijving { get; set; }
    public bool EenvoudigeAlert { get; set; }
    //Foreign keys
    //GemonitordItem: Het GemonitordItem waarop de Alert gezet werd.
    //Gebruiker: De Gebruiker die de Alert aanmaakte.
    public int GemonitordItemId { get; set; }
    public GemonitordItem GemonitordItem { get; set; }
    public ApplicationUser Gebruiker { get; set; }
    public int DeelplatformId { get; set; }
    public Deelplatform Deelplatform { get; set; }

    public Alert()
    {
      Geactiveerd = true;
      Triggered = false;
      TriggerRedenen = "";
    }
  }
}