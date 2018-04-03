using DAL;
using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
  public class GrafiekenManager
  {
    private readonly GrafiekenRepository repository;

    public GrafiekenManager()
    {
      repository = new GrafiekenRepository();
    }

    public void AddGrafiek(Grafiek grafiek)
    {
      repository.CreateGrafiek(grafiek);
    }

    public IEnumerable<Grafiek> GetGrafieken(bool dashboard = false, bool items = false)
    {
      return repository.ReadGrafieken(dashboard, dashboard);
    }

    public Grafiek GetGrafiek(int id, bool dashboard = false, bool items = false)
    {
      return repository.ReadGrafiek(id, dashboard, items);
    }

    public void ChangeGrafiek(Grafiek grafiek)
    {
      repository.UpdateGrafiek(grafiek);
    }

    public void RemoveGebruiker(Grafiek grafiek)
    {
      repository.DeleteGrafiek(grafiek);
    }
  }
}
