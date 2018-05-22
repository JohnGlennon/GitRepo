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
    private DbContext context;

    internal DbContext Context
    {
      get
      {
        if (context == null) context = new DbContext(true);
        return context;
      }
    }

    public void CommitChanges()
    {
      context.CommitChanges();
    }
  }
}
