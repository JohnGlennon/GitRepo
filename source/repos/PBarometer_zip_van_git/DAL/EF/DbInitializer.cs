using Domain.Dashboards;
using Domain.Deelplatformen;
using Domain.Gemonitordeitems;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace DAL.EF
{
  internal class DbInitializer : DropCreateDatabaseIfModelChanges<DbContext>
  {
    protected override void Seed(DbContext context)
    {
      Deelplatform deelplatform = new Deelplatform()
      {
        Naam = "Politieke Barometer",
        LaatsteSynchronisatie = DateTime.Now.AddYears(-100),
        AantalDagenHistoriek = 14,
        URLnaam = "politiek"
      };



      context.Deelplatformen.Add(deelplatform);
      context.SaveChanges();

      context.SaveChanges();
    }
  }
}
