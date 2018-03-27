using DAL;
using Domain.Gemonitordeitems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
  public class GemonitordeItemsManager
  {
    private readonly GemonitordeItemsRepository repository;

    public GemonitordeItemsManager()
    {
      repository = new GemonitordeItemsRepository();
    }

    public void AddGemonitordItem(GemonitordItem gemonitordItem)
    {
      repository.CreateGemonitordItem(gemonitordItem);
    }

    public IEnumerable<GemonitordItem> GetGemonitordeItems(bool alerts = false, bool grafieken = false)
    {
      return repository.ReadGemonitordeItems(alerts, grafieken);
    }

    public GemonitordItem GetGemonitordItem(int id, bool alerts = false, bool grafieken = false)
    {
      return repository.ReadGemonitordItem(id, alerts, grafieken);
    }

    public void ChangeGemonitordItem(GemonitordItem gemonitordItem)
    {
      repository.UpdateGemonitordItem(gemonitordItem);
    }

    public void RemoveGemonitordItem(GemonitordItem gemonitordItem)
    {
      repository.DeleteGemonitordItem(gemonitordItem);
    }
  }
}
