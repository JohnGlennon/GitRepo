using DAL;
using Domain.Dashboards;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
  public class DashboardsManager
    {
        private DashboardsRepository repository;
        private UnitOfWorkManager uowManager;

        public DashboardsManager()
        {
            InitNonExistingRepo();
            repository = new DashboardsRepository();
        }

        public void AddDashboard(Dashboard dashboard)
        {
            InitNonExistingRepo();
            repository.CreateDashboard(dashboard);
        }

        public IEnumerable<Dashboard> GetDashboards(bool gebruiker = false, bool grafieken = false)
        {
            InitNonExistingRepo();
            return repository.ReadDashboards(gebruiker, grafieken);
        }

        public Dashboard GetDashboard(int id, bool gebruiker = false, bool grafieken = false)
        {
            InitNonExistingRepo();
            return repository.ReadDashboard(id, gebruiker, grafieken);
        }
        public Dashboard GetDashboardVanGebruikerMetGrafieken(string gebruikersId, int deelplatformId)
        {
            InitNonExistingRepo();
            return repository.ReadDashboards(true, true).Where(a => a.Gebruiker != null && a.Gebruiker.Id.Equals(gebruikersId) && a.DeelplatformId == deelplatformId).FirstOrDefault();
        }

        public void ChangeDashboard(Dashboard dashboard)
        {
            InitNonExistingRepo();
            repository.UpdateDashboard(dashboard);
        }

        public void RemoveDashboard(Dashboard dashboard)
        {
            InitNonExistingRepo();
            repository.DeleteDashboard(dashboard);
        }

        public void RemoveDashboards(IEnumerable<Dashboard> dashboards)
        {
            InitNonExistingRepo(true);
            repository.DeleteDashboards(dashboards);
        }

        public void InitNonExistingRepo(bool uow = false)
        {
            if (uow)
            {
                if (uowManager == null)
                {
                    uowManager = new UnitOfWorkManager();
                    repository = new DashboardsRepository(uowManager.UnitOfWork);
                }
            }
            else
            {
                repository = repository ?? new DashboardsRepository();
            }
        }
    }
}
