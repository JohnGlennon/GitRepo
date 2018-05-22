using DAL;
using Domain.Dashboards;
using Domain.Gemonitordeitems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
  public class GrafiekenManager
  {
    private GrafiekenRepository repository;
    private UnitOfWorkManager uowManager;


    public GrafiekenManager()
    {
    }

    public void AddGrafiek(Grafiek grafiek)
    {
      InitNonExistingRepo();
      repository.CreateGrafiek(grafiek);
    }

    //public IEnumerable<Grafiek> GetGrafieken(bool dashboard = false, bool items = false)
    //{
    //  InitNonExistingRepo();
    //  return repository.ReadGrafieken(dashboard, items);
    //}

    //public IEnumerable<Grafiek> GetGrafieken(int dashboardId, int deelplatformId)
    //{
    //  InitNonExistingRepo();
    //  return repository.ReadGrafieken(dashboardId, deelplatformId);
    //}

    public IEnumerable<Grafiek> GetGrafieken(int dashboardId, int deelplatformId)
    {
      InitNonExistingRepo();
      GemonitordeItemsManager itemsManager = new GemonitordeItemsManager();
      List<GemonitordItem> alleItems = itemsManager.GetGemonitordeItems(deelplatformId).ToList();
      List<Grafiek> grafieken = repository.ReadGrafieken(dashboardId, false).ToList();

      foreach (Grafiek grafiek in grafieken)
      {
        List<GemonitordItem> grafiekItems = new List<GemonitordItem>();

        #region Kijk na welke items tot de grafiek behoren, vul de grafiekItems met items en hun historieken
        foreach (GrafiekItem item in grafiek.GrafiekItems)
        {
          foreach (GemonitordItem gevuldItem in alleItems)
          {
            if (gevuldItem.GemonitordItemId == item.GrafiekItemId)
            {
              grafiekItems.Add(gevuldItem);
            }
          }
        }
        #endregion

        #region Voeg de waarden van de historieken en plaats ze in de List
        DateTime huidigeTijd = DateTime.Now;
        if (grafiek.Type.Equals("line"))
        {

          foreach (GemonitordItem item in grafiekItems)
          {
            List<ItemHistoriek> historieken = new List<ItemHistoriek>();
            List<double> waarden = new List<double>();
            foreach (ItemHistoriek historiek in item.ItemHistorieken)
            {
              if ((huidigeTijd - historiek.HistoriekDatum).Days <= grafiek.Periode)
              {
                historieken.Add(historiek);
              }
            }

            foreach (ItemHistoriek historiek in historieken)
            {
              if (grafiek.GrafiekWaarde == GrafiekWaarde.Vermeldingen) waarden.Add(historiek.AantalVermeldingen);
              if (grafiek.GrafiekWaarde == GrafiekWaarde.Polariteit) waarden.Add(historiek.GemPolariteit);
              if (grafiek.GrafiekWaarde == GrafiekWaarde.Objectiviteit) waarden.Add(historiek.GemObjectiviteit);
              if (!grafiek.XLabels.Contains(historiek.HistoriekDatum)) grafiek.XLabels.Add(historiek.HistoriekDatum);
            }
            grafiek.LegendeLijst.Add(item.Naam);
            grafiek.Datawaarden.Add(waarden);
          }
        }

        if (grafiek.Type.Equals("bar"))
        {
          List<ItemHistoriek> historieken = new List<ItemHistoriek>();
          List<double> waarden = new List<double>();
          foreach (GemonitordItem item in grafiekItems)
          {
            foreach (ItemHistoriek historiek in item.ItemHistorieken)
            {
              if ((huidigeTijd - historiek.HistoriekDatum).Days <= grafiek.Periode)
              {
                historieken.Add(historiek);
              }
            }
            grafiek.XLabels.Add(item.Naam);
            grafiek.LegendeLijst.Add(item.Naam);
          }

          foreach (ItemHistoriek historiek in historieken)
          {
            if (grafiek.GrafiekWaarde == GrafiekWaarde.Vermeldingen) waarden.Add(historiek.AantalVermeldingen);
            if (grafiek.GrafiekWaarde == GrafiekWaarde.Polariteit) waarden.Add(historiek.GemPolariteit);
            if (grafiek.GrafiekWaarde == GrafiekWaarde.Objectiviteit) waarden.Add(historiek.GemObjectiviteit);
          }
          grafiek.Datawaarden.Add(waarden);
        }

        if (grafiek.Type.Equals("pie"))
        {
          List<ItemHistoriek> historieken = new List<ItemHistoriek>();
          List<double> waarden = new List<double>();
          foreach (GemonitordItem item in grafiekItems)
          {
            foreach (ItemHistoriek historiek in item.ItemHistorieken)
            {
              if ((huidigeTijd - historiek.HistoriekDatum).Days <= grafiek.Periode)
              {
                historieken.Add(historiek);
              }
            }
            grafiek.XLabels.Add(item.Naam);
            grafiek.LegendeLijst.Add(item.Naam);
          }

          foreach (ItemHistoriek historiek in historieken)
          {
            if (grafiek.GrafiekWaarde == GrafiekWaarde.Vermeldingen) waarden.Add(historiek.AantalVermeldingen);
            if (grafiek.GrafiekWaarde == GrafiekWaarde.Polariteit) waarden.Add(historiek.GemPolariteit);
            if (grafiek.GrafiekWaarde == GrafiekWaarde.Objectiviteit) waarden.Add(historiek.GemObjectiviteit);
          }
          grafiek.Datawaarden.Add(waarden);
        }
        #endregion
      }

      return grafieken;
    }

    //public IEnumerable<Grafiek> GetGrafieken(int dashboardId, int deelplatformId)
    //{
    //  InitNonExistingRepo();
    //  return repository.ReadGrafieken(dashboard, items).Where(a => a.Dashboard != null && a.Dashboard.DashboardId.Equals(dashboardId));
    //}


    //public List<Grafiek> GetGrafieken(int deelplatformId, int dashboardId, bool dashboard = false, bool items = false)
    //{
    //  InitNonExistingRepo();
    //  GemonitordeItemsManager itemManager = new GemonitordeItemsManager();
    //  List<GemonitordItem> alleItems = itemManager.GetGemonitordeItems(deelplatformId).ToList();
    //  List<Grafiek> grafieken = repository.ReadGrafieken(dashboardId, dashboard, items).ToList();

    //  foreach (Grafiek grafiek in grafieken)
    //  {
    //    List<GemonitordItem> grafiekItems = new List<GemonitordItem>();

    //    #region Kijk na welke items tot de grafiek behoren, vul de grafiekItems met items en hun historieken
    //    foreach (GrafiekItem item in grafiek.GrafiekItems)
    //    {
    //      foreach (GemonitordItem gevuldItem in alleItems)
    //      {
    //        if (gevuldItem.GemonitordItemId == item.GrafiekItemId)
    //        {
    //          grafiekItems.Add(gevuldItem);
    //          grafiek.Items.Add(gevuldItem); //Mogelijk niet nodig
    //        }
    //      }
    //    }
    //    #endregion

    //    #region Zoek naar de benodigde historieken en bereken de waarden die in de Dictionary moeten komen
    //    int sleutel = 0;
    //    DateTime huidigeTijd = DateTime.Now;
    //    foreach (GemonitordItem item in grafiekItems)
    //    {
    //      List<ItemHistoriek> historieken = new List<ItemHistoriek>();
    //      List<dynamic> waarden = new List<dynamic>();

    //      #region Zoek naar de historieken
    //      foreach (ItemHistoriek historiek in item.ItemHistorieken)
    //      {
    //        if ((huidigeTijd - historiek.HistoriekDatum).Days <= grafiek.Periode)
    //        {
    //          historieken.Add(historiek);
    //        }
    //      }
    //      #endregion

    //      #region Voeg de waarden van de historieken en plaats ze in de Dictionary
    //      GrafiekWaarde huidigeWaarde = grafiek.Waarden.ElementAt(sleutel);

    //      if (grafiek.Keuze == GrafiekKeuze.EvolutieAantalVermeldingen1Item)
    //      {
    //        foreach (ItemHistoriek historiek in historieken)
    //        {
    //          waarden.Add(historiek.AantalVermeldingen);
    //          grafiek.XLabels.Add(historiek.HistoriekDatum);
    //        }
    //        //grafiek.Type = GrafiekType.line;

    //        grafiek.Type = "line";
    //        grafiek.LegendeLijst.Add(item.Naam);
    //      }

    //      if (grafiek.Keuze == GrafiekKeuze.VergelijkingItemsDoorheenDeTijd)
    //      {
    //        foreach (ItemHistoriek historiek in historieken)
    //        {
    //          if (huidigeWaarde == GrafiekWaarde.Vermeldingen) waarden.Add(historiek.AantalVermeldingen);
    //          if (huidigeWaarde == GrafiekWaarde.Polariteit) waarden.Add(historiek.GemPolariteit);
    //          if (huidigeWaarde == GrafiekWaarde.Objectiviteit) waarden.Add(historiek.GemObjectiviteit);
    //          grafiek.XLabels.Add(historiek.HistoriekDatum);
    //        }
    //        grafiek.Type = "line";
    //        grafiek.LegendeLijst.Add(item.Naam + " - " + huidigeWaarde);
    //      }

    //      if (grafiek.Keuze == GrafiekKeuze.VergelijkingItemsOp1Moment)
    //      {
    //        foreach (ItemHistoriek historiek in historieken)
    //        {
    //          if (huidigeWaarde == GrafiekWaarde.Vermeldingen) waarden.Add(historiek.AantalVermeldingen);
    //          if (huidigeWaarde == GrafiekWaarde.Polariteit) waarden.Add(historiek.GemPolariteit);
    //          if (huidigeWaarde == GrafiekWaarde.Objectiviteit) waarden.Add(historiek.GemObjectiviteit);
    //          grafiek.XLabels.Add(item.Naam);
    //        }
    //        grafiek.Type = "bar";
    //      }

    //      if (grafiek.Keuze == GrafiekKeuze.KruisingTaart)
    //      {
    //        grafiek.Type = "pie";
    //      }

    //      if (grafiek.Keuze == GrafiekKeuze.KruisingBar)
    //      {
    //        grafiek.Type = "bar";
    //      }

    //      //grafiek.Data.Add(sleutel, waarden);
    //      //Sleutel verhogen na het toevoegen om bugs te vermijden
    //      ++sleutel;
    //      #endregion
    //    }
    //    #endregion
    //  }

    //  return grafieken;
    //}

    //public IEnumerable<Grafiek> GetGrafieken(int dashboardId, bool dashboard = false, bool items = false)
    //{
    //  InitNonExistingRepo();
    //  return repository.ReadGrafieken(dashboard, items).Where(a => a.Dashboard != null && a.Dashboard.DashboardId.Equals(dashboardId));
    //}



    public Grafiek GetGrafiek(int id, bool dashboard = false, bool items = false)
    {
      InitNonExistingRepo();
      return repository.ReadGrafiek(id, dashboard, items);
    }

    public void ChangeGrafiek(Grafiek grafiek)
    {
      InitNonExistingRepo();
      repository.UpdateGrafiek(grafiek);
    }

    public void RemoveGrafiek(Grafiek grafiek)
    {
      InitNonExistingRepo();
      repository.DeleteGrafiek(grafiek);
    }
    public void InitNonExistingRepo(bool uow = false)
    {
      if (uow)
      {
        if (uowManager == null)
        {
          uowManager = new UnitOfWorkManager();
          repository = new GrafiekenRepository(uowManager.UnitOfWork);
        }
      }
      else
      {
        repository = repository ?? new GrafiekenRepository();
      }
    }




    public List<Grafiek> GetGrafiekenTest()
    {

      GemonitordeItemsManager itemManager = new GemonitordeItemsManager();

      List<GemonitordItem> personen = itemManager.GetPersonen(1).ToList();
      List<dynamic> grafiek1XLabels = new List<dynamic>();
      List<double> grafiek1Datawaarden = new List<double>();

      List<GemonitordItem> grafiek1GemonitordeItems = new List<GemonitordItem>();
      List<GrafiekItem> grafiek1Grafiekitems = new List<GrafiekItem>();

      for (int i = 0; i < 5; i++)
      {
        grafiek1GemonitordeItems.Add(personen[i]);
      }

      foreach (var item in grafiek1GemonitordeItems)
      {
        grafiek1XLabels.Add(item.Naam);
        grafiek1Datawaarden.Add(item.GemPolariteit);
        grafiek1Grafiekitems.Add(new GrafiekItem()
        {
          ItemId = item.GemonitordItemId
        });
      };





      //List<ItemHistoriek> grafiek2Itemhistorieken = new List<ItemHistoriek>();
      //List<dynamic> grafiek2XLabels = new List<dynamic>();
      //List<double> grafiek2Waarden = new List<double>();

      //List<GemonitordItem> personen2 = itemManager.GetPersonen(1).ToList();

      //for (int i = 0; i < personen2.Count; i++)
      //{
      //  if (personen2[0].GemonitordItemId == 2)
      //  {
      //    grafiek2Itemhistorieken = personen2[i].ItemHistorieken;

      //  }
      //}


      //for (int i = grafiek2Itemhistorieken.Count - 10; i < grafiek2Itemhistorieken.Count; i++)
      //{
      //  grafiek2XLabels.Add(grafiek2Itemhistorieken[i].HistoriekDatum.ToShortDateString());
      //  grafiek2Waarden.Add(grafiek2Itemhistorieken[i].AantalVermeldingen);
      //}
      ////for (int i = 0; i < grafiek2Itemhistorieken.Count; i++)
      ////{
      ////  grafiek2XLabels.Add(grafiek2Itemhistorieken[i].HistoriekDatum.ToShortDateString());
      ////  grafiek2Waarden.Add(grafiek2Itemhistorieken[i].AantalVermeldingen);
      ////}





      //List<GemonitordItem> organisaties = itemManager.GetOrganisaties(1).ToList();
      //List<dynamic> grafiek3XLabels = new List<dynamic>();
      //List<ItemHistoriek> grafiek3Itemhistorieken = new List<ItemHistoriek>();
      //List<double> grafiek3Waarden = new List<double>();

      ////items = itemManager.GetGemonitordeItems(deelplatformId).ToList();
      //for (int i = 0; i < organisaties.Count; i++)
      //{
      //  if (organisaties[i].GemonitordItemId == 5)
      //  {
      //    grafiek3Itemhistorieken = organisaties[i].ItemHistorieken;
      //  }

      //}


      //for (int i = grafiek3Itemhistorieken.Count - 7; i < grafiek3Itemhistorieken.Count; i++)
      //{
      //  grafiek3XLabels.Add(grafiek3Itemhistorieken[i].HistoriekDatum.ToShortDateString());
      //  grafiek3Waarden.Add(grafiek3Itemhistorieken[i].AantalVermeldingen);
      //}
      ////for (int i = 0; i < grafiek3Itemhistorieken.Count; i++)
      ////{
      ////  grafiek3XLabels.Add(grafiek3Itemhistorieken[i].HistoriekDatum.ToShortDateString());
      ////  grafiek3Waarden.Add(grafiek3Itemhistorieken[i].AantalVermeldingen);
      ////}







      //var grafiek4items = itemManager.GetPersonen(1).ToList();

      //List<dynamic> grafiek4XLabels = new List<dynamic>();
      //List<ItemHistoriek> grafiek4Itemhistorieken = new List<ItemHistoriek>();
      //List<double> grafiek4Waarden = new List<double>();
      //List<string> grafiek4Legendelijst = new List<string>();

      //List<ItemHistoriek> itemhistoriekItem1Grafiek4 = new List<ItemHistoriek>();
      //List<ItemHistoriek> itemhistoriekItem2Grafiek4 = new List<ItemHistoriek>();
      //List<ItemHistoriek> itemhistoriekItem3Grafiek4 = new List<ItemHistoriek>();
      //List<ItemHistoriek> itemhistoriekItem4Grafiek4 = new List<ItemHistoriek>();
      //List<ItemHistoriek> itemhistoriekItem5Grafiek4 = new List<ItemHistoriek>();

      //List<double> waardenItem1Grafiek4 = new List<double>();
      //List<double> waardenItem2Grafiek4 = new List<double>();
      //List<double> waardenItem3Grafiek4 = new List<double>();
      //List<double> waardenItem4Grafiek4 = new List<double>();
      //List<double> waardenItem5Grafiek4 = new List<double>();

      //List<List<double>> alleWaarden = new List<List<double>>();



      //foreach (var element in grafiek4items)
      //{
      //  if (element.GemonitordItemId == 2)
      //  {
      //    itemhistoriekItem1Grafiek4 = element.ItemHistorieken;
      //    grafiek4Legendelijst.Add(element.Naam);
      //  }

      //  if (element.GemonitordItemId == 3)
      //  {
      //    itemhistoriekItem2Grafiek4 = element.ItemHistorieken;
      //    grafiek4Legendelijst.Add(element.Naam);
      //  }

      //  if (element.GemonitordItemId == 4)
      //  {
      //    itemhistoriekItem3Grafiek4 = element.ItemHistorieken;
      //    grafiek4Legendelijst.Add(element.Naam);
      //  }

      //  if (element.GemonitordItemId == 6)
      //  {
      //    itemhistoriekItem4Grafiek4 = element.ItemHistorieken;
      //    grafiek4Legendelijst.Add(element.Naam);
      //  }

      //  if (element.GemonitordItemId == 7)
      //  {
      //    itemhistoriekItem5Grafiek4 = element.ItemHistorieken;
      //    grafiek4Legendelijst.Add(element.Naam);
      //  }

      //}


      //for (int i = itemhistoriekItem4Grafiek4.Count - 5; i < itemhistoriekItem4Grafiek4.Count; i++)
      //{
      //  grafiek4XLabels.Add(itemhistoriekItem1Grafiek4[i].HistoriekDatum.ToShortDateString());

      //  waardenItem1Grafiek4.Add(itemhistoriekItem1Grafiek4[i].AantalVermeldingen);
      //  waardenItem2Grafiek4.Add(itemhistoriekItem2Grafiek4[i].AantalVermeldingen);
      //  waardenItem3Grafiek4.Add(itemhistoriekItem3Grafiek4[i].AantalVermeldingen);
      //  waardenItem4Grafiek4.Add(itemhistoriekItem4Grafiek4[i].AantalVermeldingen);
      //  waardenItem5Grafiek4.Add(itemhistoriekItem5Grafiek4[i].AantalVermeldingen);

      //}

      ////for (int i = 0; i < itemhistoriekItem1Grafiek4.Count; i++)
      ////{
      ////  grafiek4XLabels.Add(itemhistoriekItem1Grafiek4[i].HistoriekDatum.ToShortDateString());

      ////  waardenItem1Grafiek4.Add(itemhistoriekItem1Grafiek4[i].AantalVermeldingen);
      ////  waardenItem2Grafiek4.Add(itemhistoriekItem2Grafiek4[i].AantalVermeldingen);
      ////  waardenItem3Grafiek4.Add(itemhistoriekItem3Grafiek4[i].AantalVermeldingen);
      ////  waardenItem4Grafiek4.Add(itemhistoriekItem4Grafiek4[i].AantalVermeldingen);
      ////  waardenItem5Grafiek4.Add(itemhistoriekItem5Grafiek4[i].AantalVermeldingen);

      ////}


      //alleWaarden.Add(waardenItem1Grafiek4);
      //alleWaarden.Add(waardenItem2Grafiek4);
      //alleWaarden.Add(waardenItem3Grafiek4);
      //alleWaarden.Add(waardenItem4Grafiek4);
      //alleWaarden.Add(waardenItem5Grafiek4);


      List<Grafiek> grafieken = new List<Grafiek>()
      {

      new Grafiek()
      {

        DeelplatformId = 1,
        DashboardId = 1,

        Titel = "Vergelijking op moment - gemiddelde polariteit",
        ToonLegende = false,
        ToonXAs = true,
        ToonYAs = true,

        Type = "bar",

        XOorsprongNul     = true,
        XTitel            = "Items",
        YOorsprongNul     = true,
        YTitel            = "Waarden",
        XLabels = grafiek1XLabels,
        Datawaarden = new List<List<double>>(){ grafiek1Datawaarden },


      Achtergrondkleur = new List<List<string>>(){ new List<string> { "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850", "#ffff00", "#ffa500" }, null, null, null, null},
        Randkleur = new List<List<string>>(){ new List<string> { "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850", "#ffff00", "#ffa500" }, null, null, null, null},
        LegendeLijst = new List<dynamic>{ null, null, null, null, null },

        XAsMaxrotatie = 90,
        XAsMinrotatie = 90,
        FillDataset = false,
        Lijnlegendeweergave = false,

        //Items = grafiek1GemonitordeItems,
        GrafiekItems = grafiek1Grafiekitems,

        GrafiekWaarde = GrafiekWaarde.Polariteit,

      },
      //new Grafiek()
      //{
      //  GrafiekId = 2,

      //  DeelplatformId = 1,
      //  DashboardId = 1,

      //  Titel = "aantal tweets",
      //  ToonLegende = false,
      //  ToonXAs = true,
      //  ToonYAs = true,

      //  Type = "line",

      //  XOorsprongNul     = true,
      //  XTitel            = "Items",
      //  YOorsprongNul     = true,
      //  YTitel            = "Waarden",
      //  XLabels = grafiek2XLabels,

      //  Datawaarden = new List<List<double>>(){ grafiek2Waarden},

      //  Achtergrondkleur = new List<List<string>>(){ new List<string> { "#3e95cd" }, null, null, null, null},
      //  Randkleur = new List<List<string>>(){ new List<string> { "#3e95cd" }, null, null, null, null},
      //  LegendeLijst = new List<string>{ null, null, null, null, null },

      //  XAsMaxrotatie = 90,
      //  XAsMinrotatie = 90,
      //  FillDataset = false,
      //  Lijnlegendeweergave = false

      //},

      // new Grafiek()
      //{
      //  GrafiekId = 3,

      //  DeelplatformId = 1,
      //  DashboardId = 1,

      //  Titel = "aantal tweets",
      //  ToonLegende = false,
      //  ToonXAs = true,
      //  ToonYAs = true,

      //  Type = "line",

      //  XOorsprongNul     = true,
      //  XTitel            = "Items",
      //  YOorsprongNul     = true,
      //  YTitel            = "Waarden",
      //  XLabels = grafiek3XLabels,

      //  Datawaarden = new List<List<double>>(){ grafiek3Waarden},

      //  Achtergrondkleur = new List<List<string>>(){ new List<string> { "#3e95cd" }, null, null, null, null},
      //  Randkleur = new List<List<string>>(){ new List<string> { "#3e95cd" }, null, null, null, null},
      //  LegendeLijst = new List<string>{ null, null, null, null, null },

      //  XAsMaxrotatie = 90,
      //  XAsMinrotatie = 90,
      //  FillDataset = false,
      //  Lijnlegendeweergave = false

      //},
      // new Grafiek()
      //{
      //  GrafiekId = 4,

      //  DeelplatformId = 1,
      //  DashboardId = 1,

      //  Titel = "aantal tweets",
      //  ToonLegende = true,
      //  ToonXAs = true,
      //  ToonYAs = true,

      //  Type = "line",

      //  XOorsprongNul     = true,
      //  XTitel            = "Items",
      //  YOorsprongNul     = true,
      //  YTitel            = "Waarden",
      //  XLabels = grafiek4XLabels,

      //  Datawaarden = alleWaarden,

      //  Achtergrondkleur = new List<List<string>>(){ new List<string> { "#3e95cd" }, new List<string> {"#8e5ea2" }, new List<string> { "#3cba9f"}, new List<string> { "#e8c3b9"}, new List<string> { "#c45850" } },
      //  Randkleur        = new List<List<string>>(){ new List<string> { "#3e95cd" }, new List<string> {"#8e5ea2" }, new List<string> { "#3cba9f"}, new List<string> { "#e8c3b9"}, new List<string> { "#c45850" } },
      //  LegendeLijst     = new List<string>{ grafiek4Legendelijst[0], grafiek4Legendelijst[1], grafiek4Legendelijst[2], grafiek4Legendelijst[3], grafiek4Legendelijst[4] },

      //  XAsMaxrotatie = 90,
      //  XAsMinrotatie = 90,
      //  FillDataset = false,
      //  Lijnlegendeweergave = true

      //}


    };

      return grafieken;

    }
  }
}