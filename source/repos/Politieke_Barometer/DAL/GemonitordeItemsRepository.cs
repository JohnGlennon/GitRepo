using DAL.EF;
using Domain.Gemonitordeitems;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public class GemonitordeItemsRepository
  {
    private readonly PBDbContext context;

    public GemonitordeItemsRepository()
    {
      context = new PBDbContext();
    }

    public void CreateGemonitordItem(GemonitordItem gemonitordItem)
    {
      context.GemonitordeItems.Add(gemonitordItem);
      context.SaveChanges();
    }

    public IEnumerable<GemonitordItem> ReadGemonitordeItems(bool alerts, bool grafieken)
    {
      if (!alerts && !grafieken) return context.GemonitordeItems.Include("DetailItems").AsEnumerable();
      if (!alerts && grafieken) return context.GemonitordeItems.Include("Grafieken").Include("DetailItems").AsEnumerable();
      if (alerts && !grafieken) return context.GemonitordeItems.Include("Alerts").Include("DetailItems").AsEnumerable();
      else return context.GemonitordeItems.Include("Alerts").Include("Grafieken").Include("DetailItems").AsEnumerable();
    }

    public GemonitordItem ReadGemonitordItem(int id, bool alerts, bool grafieken)
    {
      if (!alerts && !grafieken) return context.GemonitordeItems.Include("DetailItems").AsEnumerable().SingleOrDefault(i => i.GemonitordItemId == id);
      if (!alerts && grafieken) return context.GemonitordeItems.Include("Grafieken").Include("DetailItems").AsEnumerable().SingleOrDefault(i => i.GemonitordItemId == id);
      if (alerts && !grafieken) return context.GemonitordeItems.Include("Alerts").Include("DetailItems").AsEnumerable().SingleOrDefault(i => i.GemonitordItemId == id);
      else return context.GemonitordeItems.Include("Alerts").Include("Grafieken").Include("DetailItems").AsEnumerable().SingleOrDefault(i => i.GemonitordItemId == id);
    }

    public void UpdateGemonitordItem(GemonitordItem gemonitordItem)
    {
      context.Entry(gemonitordItem).State = EntityState.Modified;
      context.SaveChanges();
    }

    public void DeleteGemonitordItem(GemonitordItem gemonitordItem)
    {
      context.GemonitordeItems.Remove(gemonitordItem);
      context.SaveChanges();
    }
  }
}
