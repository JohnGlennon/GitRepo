using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Gemonitordeitems
{
  public class GemonitordItem
  {
    public int GemonitordItemId { get; set; }

    public string Naam { get; set; }
    public Trend VermeldingenTrend { get; set; }
    public Trend PolariteitsTrend { get; set; }
    public Trend ObjectiviteitsTrend { get; set; }
    public List<int> AantalVermeldingen { get; set; }
    public List<double> Polariteit { get; set; }
    public List<double> Objectiviteit { get; set; }
    public List<DateTime> Datums { get; set; }

    //Foreign keys
    public List<Alert> Alerts { get; set; }
    public List<Grafiek> Grafieken { get; set; }
    public List<GemonitordItemDetail> DetailItems { get; set; }

    public GemonitordItem()
    {
      Alerts = new List<Alert>();
      Grafieken = new List<Grafiek>();
      AantalVermeldingen = new List<int>();
      Polariteit = new List<double>();
      Objectiviteit = new List<double>();
      DetailItems = new List<GemonitordItemDetail>();
      Datums = new List<DateTime>();
    }
  }
}
