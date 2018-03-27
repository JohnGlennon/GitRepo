using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public class UnitOfWork
  {
    private PBDbContext context;

    internal PBDbContext Context
    {
      get
      {
        if (context == null) context = new PBDbContext(true);
        return context;
      }
    }

    public void CommitChanges()
    {
      context.CommitChanges();
    }
  }
}
