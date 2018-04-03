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
  internal class PBDbInitializer : DropCreateDatabaseIfModelChanges<PBDbContext>
  {
    protected override void Seed(PBDbContext context)
    {
      Gebruiker jelle = new Gebruiker()
      {
        Voornaam = "Jelle",
        Naam = "Van der Donck",
        Wachtwoord = "jelle",
        Email = "jelle@kdg.be",
        Rol = Rol.SUPERADMIN
      };

      Gebruiker bart = new Gebruiker()
      {
        Voornaam = "Bart",
        Naam = "Wezenbeek",
        Wachtwoord = "bart",
        Email = "bart@kdg.be",
        Rol = Rol.ADMIN
      };

      Gebruiker arne = new Gebruiker()
      {
        Voornaam = "Arne",
        Naam = "Driesen",
        Wachtwoord = "arne",
        Email = "arne@kdg.be",
        Rol = Rol.INGELOGD
      };

      Gebruiker seppe = new Gebruiker()
      {

        Voornaam = "Seppe",
        Naam = "Lamberts",
        Wachtwoord = "seppe",
        Email = "seppe@kdg.be",
        Rol = Rol.INGELOGD
      };

      Gebruiker glenn = new Gebruiker()
      {
        Voornaam = "Glenn",
        Naam = "Geysen",
        Wachtwoord = "glenn",
        Email = "glenn@kdg.be",
        Rol = Rol.ADMIN
      };

      context.Gebruikers.Add(jelle);
      context.Gebruikers.Add(bart);
      context.Gebruikers.Add(arne);
      context.Gebruikers.Add(seppe);
      context.Gebruikers.Add(glenn);

      context.SaveChanges();
    }
  }
}
