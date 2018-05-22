using Domain.Dashboards;
using Domain.Deelplatformen;
using Domain.Gemonitordeitems;
using Domain.IdentityFramework;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DAL.EF
{
  [DbConfigurationType(typeof(DbConfiguration))]
  public class DbContext : IdentityDbContext<ApplicationUser>
  {
    private readonly bool delaySave;

    //Database Sets
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<Dashboard> Dashboards { get; set; }
    public DbSet<Grafiek> Grafieken { get; set; }
    public DbSet<Deelplatform> Deelplatformen { get; set; }

    public DbSet<GemonitordItem> GemonitordeItems { get; set; }
    public DbSet<DetailItem> DetailItems { get; set; }

    public DbContext(bool unitOfWorkPresent = false) : base("PBDb_Barometer")
    {
      delaySave = unitOfWorkPresent;
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      //base.OnModelCreating(modelBuilder);
      //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
      modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

      //Tabelnamen
      modelBuilder.Entity<Alert>().ToTable("Alerts");
      modelBuilder.Entity<Dashboard>().ToTable("Dashboards");
      modelBuilder.Entity<Grafiek>().ToTable("Grafieken");
      modelBuilder.Entity<GrafiekItem>().ToTable("GrafiekDetails");
      modelBuilder.Entity<DetailItem>().ToTable("DetailItems");
      modelBuilder.Entity<GemonitordItem>().ToTable("GemonitordeItems");
      modelBuilder.Entity<ItemHistoriek>().ToTable("ItemHistorieken");
      modelBuilder.Entity<GrafiekItem>().ToTable("Grafiekitems");

      modelBuilder.Entity<IdentityUserLogin>().HasKey(l => l.UserId);
      modelBuilder.Entity<IdentityRole>().HasKey(r => r.Id);
      modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

      modelBuilder.Entity<Grafiek>().HasKey(g => g.GrafiekId);
      //modelBuilder.Entity<GrafiekItem>().HasKey(gi => gi.GrafiekItemId);

      //Foreign keys
      //modelBuilder.Entity<Alert>().HasRequired(alert => alert.Gebruiker).WithMany(gebruiker => gebruiker.Alerts);
      //modelBuilder.Entity<Gebruiker>().HasMany(gebruiker => gebruiker.Alerts).WithRequired(alert => alert.Gebruiker);

      //  modelBuilder.Entity<Alert>().HasRequired(alert => alert.GemonitordItem).WithMany(gemonitordItem => gemonitordItem.Alerts);
      modelBuilder.Entity<GemonitordItem>().HasMany(gemonitordItem => gemonitordItem.Alerts).WithRequired(alert => alert.GemonitordItem).WillCascadeOnDelete(true);
            //modelBuilder.Entity<ApplicationUser>().HasOptional(gebruiker => gebruiker.Dashboard).WithRequired(dashboard => dashboard.Gebruiker);

            //  modelBuilder.Entity<Dashboard>().HasMany(dashboard => dashboard.Grafieken).WithRequired(grafiek => grafiek.Dashboard);
            //  modelBuilder.Entity<Grafiek>().HasRequired(grafiek => grafiek.Dashboard).WithMany(dashboard => dashboard.Grafieken);

            //  modelBuilder.Entity<Grafiek>().HasMany(grafiek => grafiek.GemonitordeItems).WithMany(gemonitordItem => gemonitordItem.Grafieken);
            //  modelBuilder.Entity<GemonitordItem>().HasMany(gemonitordItem => gemonitordItem.Grafieken).WithMany(grafiek => grafiek.GemonitordeItems);

            //  modelBuilder.Entity<GemonitordItem>().HasMany(gemonitordItem => gemonitordItem.DetailItems).WithRequired(detailItem => detailItem.GemonitordeItems);
            //  modelBuilder.Entity<DetailItem>().HasRequired(detailItem => detailItem.GemonitordItem).WithMany(gemonitordItem => gemonitordItem.DetailItems);

            //  modelBuilder.Entity<GemonitordItem>().HasMany(gemonitordItem => gemonitordItem.ItemHistorieken).WithRequired(itemHistoriek => itemHistoriek.GemonitordItem);
            //  modelBuilder.Entity<ItemHistoriek>().HasRequired(itemHistoriek => itemHistoriek.GemonitordItem).WithMany(gemonitordItem => gemonitordItem.ItemHistorieken);
            modelBuilder.Entity<Deelplatform>().Property(a => a.URLnaam).HasMaxLength(20);
            modelBuilder.Entity<Deelplatform>().HasIndex(deelplatform => deelplatform.URLnaam).IsUnique();

            //modelBuilder.Entity<Deelplatform>(). .WithOptional(dashboard => dashboard.Deelplatform).WillCascadeOnDelete(true);
            //modelBuilder.Entity<Deelplatform>().HasMany(deelplatform => deelplatform.GemonitordeItems).WithRequired(item => item.Deelplatform);
            //modelBuilder.Entity<Deelplatform>().HasMany(deelplatform => deelplatform.Alerts).WithRequired(alert => alert.Deelplatform);
            //modelBuilder.Entity<Deelplatform>().HasMany(deelplatform => deelplatform.DetailItems).WithRequired(detailitem => detailitem.Deelplatform);
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