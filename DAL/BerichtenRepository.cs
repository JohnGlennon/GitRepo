using DAL.EF;
using Domain.Bericht;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public class BerichtenRepository
  {
    private readonly PBDbContext context;

    public BerichtenRepository()
    {
      context = new PBDbContext();
    }

    public void CreateBericht(Bericht bericht)
    {
      context.Berichten.Add(bericht);
      context.SaveChanges();
    }

    public IEnumerable<Bericht> ReadBerichten()
    {
      return context.Berichten.AsEnumerable();
    }

    public Bericht ReadBericht(int id)
    {
      return context.Berichten.SingleOrDefault(b => b.BerichtId == id);
    }

    public void UpdateBericht(Bericht bericht)
    {
      context.Entry(bericht).State = EntityState.Modified;
      context.SaveChanges();
    }

    public void DeleteBericht(Bericht bericht)
    {
      context.Berichten.Remove(bericht);
      context.SaveChanges();
    }
  }
}
