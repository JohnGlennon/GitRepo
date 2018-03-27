using Domain.Bericht;
using Domain.Dashboards;
using Domain.Gebruikers;
using Domain.Gemonitordeitems;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
  [DbConfigurationType(typeof(PBDbConfiguration))]
  internal class PBDbContext : DbContext
  {
    private readonly bool delaySave;

    //Database Sets
    public DbSet<Bericht> Berichten { get; set; }
    
    public DbSet<Gebruiker> Gebruikers { get; set; }

    public DbSet<Alert> Alerts { get; set; }
    public DbSet<Dashboard> Dashboards { get; set; }
    public DbSet<Grafiek> Grafieken { get; set; }
    
    public DbSet<GemonitordItem> GemonitordeItems { get; set; }

    public PBDbContext(bool unitOfWorkPresent = false) : base("PBDb_Barometer")
    {
      delaySave = unitOfWorkPresent;
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      //base.OnModelCreating(modelBuilder);
      //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
      //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

      //Primary keys
      modelBuilder.Entity<Alert>().HasKey(alert => alert.AlertId);
      modelBuilder.Entity<Bericht>().HasKey(bericht => bericht.BerichtId);
      modelBuilder.Entity<Dashboard>().HasKey(dashboard => dashboard.DashboardId);
      modelBuilder.Entity<Grafiek>().HasKey(grafiek => grafiek.GrafiekId);
      modelBuilder.Entity<Gebruiker>().HasKey(gebruiker => gebruiker.GebruikerId);
      modelBuilder.Entity<GemonitordItem>().HasKey(gemonitordItem => gemonitordItem);
    }

    public override int SaveChanges()
    {
      if (delaySave) return -1;
      return base.SaveChanges();
    }

    internal int CommitChanges()
    {
      if (delaySave)
      {
        return base.SaveChanges();
      }
      throw new InvalidOperationException("No UnitOfWork present, use SaveChanges instead");
    }
  }
}