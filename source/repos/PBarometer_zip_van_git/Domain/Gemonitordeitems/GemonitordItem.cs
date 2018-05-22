using Domain.Dashboards;
using Domain.Deelplatformen;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Gemonitordeitems
{
  public class GemonitordItem
  {
    public int GemonitordItemId { get; set; }

    public string Naam { get; set; }

    public Trend VermeldingenTrend { get; set; }
    public Trend PolariteitsTrend { get; set; }
    public Trend ObjectiviteitsTrend { get; set; }

    public bool Volgbaar { get; set; }
    public int TotaalAantalVermeldingen { get; set; }
    public double GemPolariteit { get; set; }
    public double GemObjectiviteit { get; set; }
    public string MeestVoorkomendeURL { get; set; }
    public int AantalBerichtenVanMannen { get; set; }
    public int AantalBerichtenVanVrouwen { get; set; }
    public virtual List<ItemHistoriek> ItemHistorieken { get; set; }

    //Foreign keys
    public List<Alert> Alerts { get; set; }
    public List<DetailItem> DetailItems { get; set; }
    public Deelplatform Deelplatform { get; set; }
    public int DeelplatformId { get; set; }
    public GemonitordItem()
    {
      DetailItems = new List<DetailItem>();
      ItemHistorieken = new List<ItemHistoriek>();
      Alerts = new List<Alert>();
    }

    public virtual void BerekenEigenschappen()
    {
      if (DetailItems != null && DetailItems.Count > 0)
      {
        TotaalAantalVermeldingen = DetailItems.Count;
        BerekenGemiddeldeObjectiviteit();
        BerekenGemiddeldePolariteit();
        BepaalMeestVoorkomendeURL();
        BerekenTotaalAantalVrouwenEnMannen();
        if (ItemHistorieken != null && ItemHistorieken.Count > 0 && TotaalAantalVermeldingen > 0)
        {
          BerekenPolTrend();
          BerekenObjTrend();
          BerekenVermeldingenTrend();
        }
      }
      else
      {
        TotaalAantalVermeldingen = 0;
        PolariteitsTrend = Trend.NEUTRAL;
        ObjectiviteitsTrend = Trend.NEUTRAL;
        VermeldingenTrend = Trend.NEUTRAL;
      }

    }

    private void BerekenTotaalAantalVrouwenEnMannen()
    {
      AantalBerichtenVanMannen = DetailItems.Where(a => a.ProfielEigenschappen["gender"].Equals("m")).Count();
      AantalBerichtenVanVrouwen = DetailItems.Where(a => a.ProfielEigenschappen["gender"].Equals("f")).Count();
    }

    private void BepaalMeestVoorkomendeURL()
    {
      var url = DetailItems.Where(a => a.AndereEigenschappen["urls"].FirstOrDefault() != null).GroupBy(a => a.AndereEigenschappen["urls"].
      FirstOrDefault()).OrderByDescending(b => b.Count()).FirstOrDefault();

      if (url != null)
      {
        MeestVoorkomendeURL = url.Key;
      }
      else
      {
        MeestVoorkomendeURL = null;
      }

    }

    private void BerekenGemiddeldePolariteit()
    {
      if (DetailItems.Count > 0)
      {
        double gemiddelde = 0;
        int teller = 0;
        foreach (var item in DetailItems)
        {
          gemiddelde += item.Polariteit;
          teller += 1;
        }
        GemPolariteit = gemiddelde / teller;
      }
    }
    private void BerekenGemiddeldeObjectiviteit()
    {
      if (DetailItems.Count > 0)
      {
        double gemiddelde = 0;
        int teller = 0;
        foreach (var item in DetailItems)
        {
          gemiddelde += item.Objectiviteit;
          teller += 1;
        }
        GemObjectiviteit = gemiddelde / teller;
      }
    }
    private void BerekenPolTrend()
    {
      double gemLaatstePolariteit;
      double gemPolariteitPositief = GemPolariteit + 1;
      if (DetailItems.Count > 10)
      {
        gemLaatstePolariteit = DetailItems.OrderBy(a => a.BerichtDatum).Take(DetailItems.Count / 10).Average(a => a.Objectiviteit) + 1;
        if (gemLaatstePolariteit > 0.95 * gemPolariteitPositief && gemLaatstePolariteit < 0.95 * gemPolariteitPositief)
        {
          PolariteitsTrend = Trend.NEUTRAL;
        }
        else if (gemLaatstePolariteit > gemPolariteitPositief * 1.05)
        {
          PolariteitsTrend = Trend.UP;
        }
        else
        {
          PolariteitsTrend = Trend.DOWN;
        }
      }
      else
      {
        PolariteitsTrend = Trend.NEUTRAL;
      }
    }

    private void BerekenObjTrend()
    {
      double gemLaatsteObjectiviteit;
      if (DetailItems.Count > 10)
      {
        gemLaatsteObjectiviteit = DetailItems.OrderBy(a => a.BerichtDatum).Take(DetailItems.Count / 10).Average(a => a.Objectiviteit);
        if (gemLaatsteObjectiviteit > 0.95 * GemObjectiviteit && gemLaatsteObjectiviteit < 0.95 * GemObjectiviteit)
        {
          ObjectiviteitsTrend = Trend.NEUTRAL;
        }
        else if (gemLaatsteObjectiviteit > GemObjectiviteit * 1.05)
        {
          ObjectiviteitsTrend = Trend.UP;
        }
        else
        {
          ObjectiviteitsTrend = Trend.DOWN;
        }
      }
      else
      {
        ObjectiviteitsTrend = Trend.NEUTRAL;
      }
    }

    private void BerekenVermeldingenTrend()
    {
      int vorigAantalVermeldingen = ItemHistorieken.Last().AantalVermeldingen;
      if (TotaalAantalVermeldingen > vorigAantalVermeldingen * 0.9 && TotaalAantalVermeldingen < vorigAantalVermeldingen * 1.10)
      {
        VermeldingenTrend = Trend.NEUTRAL;
      }
      else if (TotaalAantalVermeldingen > vorigAantalVermeldingen * 1.10)
      {
        VermeldingenTrend = Trend.UP;
      }
      else
      {
        VermeldingenTrend = Trend.DOWN;
      }
    }

  }
}
