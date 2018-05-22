using DAL;
using Domain.Deelplatformen;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BL
{
  public class DeelplatformenManager
  {
    private DeelplatformenRepository repository;
    private UnitOfWorkManager uowManager;

    public DeelplatformenManager()
    {
    }

    public void AddDeelplatform(Deelplatform deelplatform)
    {
      InitNonExistingRepo();
      repository.CreateDeelplatform(deelplatform);
    }

    public IEnumerable<Deelplatform> GetDeelplatformen()
    {
      InitNonExistingRepo();
      return repository.ReadDeelplatformen();
    }

    public Deelplatform GetDeelplatformByName(string naam)
    {
      InitNonExistingRepo();
      return repository.ReadDeelplatformByName(naam);
    }
    public Deelplatform GetDeelplatform(int id, bool relationeleEntiteiten = false)
    {
      InitNonExistingRepo();
      return repository.ReadDeelplatform(id, relationeleEntiteiten);
    }
    public void ChangeDeelplatform(Deelplatform deelplatform)
    {
      InitNonExistingRepo();
      repository.UpdateDeelplatform(deelplatform);
    }

    public void RemoveDeelplatform(int id)
    {
      InitNonExistingRepo(true);
      Deelplatform deelplatform = GetDeelplatform(id, true);
      DashboardsManager dashboardsManager = new DashboardsManager();
      GemonitordeItemsManager gemonitordeItemsManager = new GemonitordeItemsManager();
      AlertManager alertManager = new AlertManager();
      dashboardsManager.RemoveDashboards(deelplatform.Dashboards);
      gemonitordeItemsManager.RemoveGemonitordeItems(deelplatform.GemonitordeItems);
      alertManager.RemoveAlerts(deelplatform.Alerts);
      repository.DeleteDeelplatform(deelplatform);
      uowManager.Save();
    }

    public Deelplatform GetDeelplatformByURL(string url)
    {
      InitNonExistingRepo();
      return repository.ReadDeelplatformen().FirstOrDefault(a => a.URLnaam.Equals(url, StringComparison.OrdinalIgnoreCase));
    }

    //Haalt op wat niet-ingelogde gebruikers kunnen zien en doen op de site.
    public Settings GetSettings(int id)
    {
      return repository.ReadSettings(id);
    }

    //Verandert of de niet-ingelogde gebruikers al dan niet het overzicht kunnen raadplegen.
    public void ChangeOverzichtAdded(int id, bool OverzichtAdded)
    {
      repository.UpdateOverzichtAdded(id, OverzichtAdded);
    }

    //Verandert of de niet-ingelogde gebruikers al dan niet de alerts kunnen raadplegen.
    public void ChangeAlertsAdded(int id, bool AlertsAdded)
    {
      repository.UpdateAlertsAdded(id, AlertsAdded);
    }

    public string GetAchtergrondkleur(int id)
    {
      return repository.ReadAchtergrondkleur(id);
    }

    public void ChangeAchtergrondkleur(int id, string kleur)
    {
      repository.UpdateAchtergrondkleur(id, kleur);
    }

    public List<FAQItem> GetFAQItems(int id)
    {
      return repository.ReadFAQItems(id);
    }

    public void AddNieuweFAQItem(int id, FAQItem NieuweFAQItem)
    {
      repository.CreateNieuweFAQItem(id, NieuweFAQItem);
    }

    public void RemoveFAQItem(int id, string vraag)
    {
      repository.DeleteFAQItem(id, vraag);
    }

    public void InitNonExistingRepo(bool uow = false)
    {
      if (uow)
      {
        if (uowManager == null)
        {
          uowManager = new UnitOfWorkManager();
          repository = new DeelplatformenRepository(uowManager.UnitOfWork);
        }
      }
      else
      {
        repository = repository ?? new DeelplatformenRepository();
      }
    }
  }
}
