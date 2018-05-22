using Domain.Gemonitordeitems;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dashboards
{
  public class Grafiek
  {

    public int GrafiekId { get; set; }

    public string Titel { get; set; }

    public bool ToonLegende { get; set; }
    public bool ToonXAs { get; set; }
    public bool ToonYAs { get; set; }

    public bool XOorsprongNul { get; set; }
    public bool YOorsprongNul { get; set; }

    public string XTitel { get; set; }
    public string YTitel { get; set; }


    [NotMapped]
    public List<dynamic> LegendeLijst { get; set; }
    public List<dynamic> XLabels { get; set; }
    //public List<string> XLabelsDatums { get; set; }
    //public List<string> XLabelsItems { get; set; }

    //[NotMapped]
    //public List<dynamic> XLabels
    //{
    //  get
    //  {
    //    return XLabelsJSON == null ? null :
    //    JsonConvert.DeserializeObject<List<dynamic>>(XLabelsJSON);
    //  }
    //  set
    //  {
    //    XLabelsJSON = JsonConvert.SerializeObject(value);
    //  }
    //}

    //public string XLabelsJSON { get; set; }



    [NotMapped]
    public List<List<double>> Datawaarden { get; set; }
    //[NotMapped]
    //public List<List<double>> Datawaarden
    //{
    //  get
    //  {
    //    return DatawaardenJSON == null ? null :
    //    JsonConvert.DeserializeObject<List<List<double>>>(DatawaardenJSON);
    //  }
    //  set
    //  {
    //    DatawaardenJSON = JsonConvert.SerializeObject(value);
    //  }
    //}

    //public string DatawaardenJSON { get; set; }





    public int Periode { get; set; }
    //[NotMapped]
    //public GrafiekType Type { get; set; }

    public string Type { get; set; }
    //public GrafiekKeuze Keuze { get; set; }

    //Foreign keys
    //Dashboard: Het Dashboard waartoe de Grafiek behoort.
    //GemonitordeItems: De GemonitordeItems die de Grafiek gebruikt.
    //public Dashboard Dashboard { get; set; }
    public int DashboardId { get; set; }
    public int DeelplatformId { get; set; }

    //[NotMapped]
    //public List<GemonitordItem> Items { get; set; }


    //public List<GrafiekItem> GrafiekItems { get; set; }
    [NotMapped]
    public List<GrafiekItem> GrafiekItems
    {
      get
      {
        return GrafiekItemsJSON == null ? null :
        JsonConvert.DeserializeObject<List<GrafiekItem>>(GrafiekItemsJSON);
      }
      set
      {
        GrafiekItemsJSON = JsonConvert.SerializeObject(value);
      }
    }

    public string GrafiekItemsJSON { get; set; }

    //public List<List<string>> Randkleur { get; set; }
    [NotMapped]
    public List<List<string>> Randkleur
    {
      get
      {
        return RandkleurJSON == null ? null :
        JsonConvert.DeserializeObject<List<List<string>>>(RandkleurJSON);
      }
      set
      {
        RandkleurJSON = JsonConvert.SerializeObject(value);
      }
    }

    public string RandkleurJSON { get; set; }



    //public List<List<string>> Achtergrondkleur { get; set; }
    [NotMapped]
    public List<List<string>> Achtergrondkleur
    {
      get
      {
        return AchtergrondkleurJSON == null ? null :
        JsonConvert.DeserializeObject<List<List<string>>>(AchtergrondkleurJSON);
      }
      set
      {
        AchtergrondkleurJSON = JsonConvert.SerializeObject(value);
      }
    }

    public string AchtergrondkleurJSON { get; set; }



    public GrafiekWaarde GrafiekWaarde { get; set; }


    public int XAsMaxrotatie { get; set; }
    public int XAsMinrotatie { get; set; }
    public bool FillDataset { get; set; }
    public bool Lijnlegendeweergave { get; set; }


    //[NotMapped]
    //public List<GrafiekWaarde> Waarden
    //{
    //  get
    //  {
    //    return WaardenJSON == null ? null : JsonConvert.DeserializeObject<List<GrafiekWaarde>>(WaardenJSON);
    //  }
    //  set
    //  {
    //    WaardenJSON = JsonConvert.SerializeObject(value);
    //  }
    //}

    //public string WaardenJSON { get; set; }

    public Grafiek()
    {
      LegendeLijst = new List<dynamic>();
      //Data = new Dictionary<int, List<dynamic>>();
      //Items = new List<GemonitordItem>();
      //GrafiekItems = new List<GrafiekItem>();
      //Waarden = new List<GrafiekWaarde>();
      Datawaarden = new List<List<double>>();
      XLabels = new List<dynamic>();
    }
  }
}
