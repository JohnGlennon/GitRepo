using DAL;
using Domain.Bericht;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
  public class BerichtenManager
  {
    private readonly BerichtenRepository repository;

    public BerichtenManager()
    {
      repository = new BerichtenRepository();
    }

    public void AddBericht(Bericht bericht)
    {
      repository.CreateBericht(bericht);
    }

    public IEnumerable<Bericht> GetBerichten()
    {
      return repository.ReadBerichten();
    }

    public Bericht GetBericht(int id)
    {
      return repository.ReadBericht(id);
    }

    public void ChangeBericht(Bericht bericht)
    {
      repository.UpdateBericht(bericht);
    }

    public void RemoveBericht(Bericht bericht)
    {
      repository.DeleteBericht(bericht);
    }
  }
}
