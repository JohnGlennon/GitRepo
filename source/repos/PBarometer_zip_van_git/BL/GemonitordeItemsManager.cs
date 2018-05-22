//Bart
using DAL;
using Domain.Gemonitordeitems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
  public class GemonitordeItemsManager
    {

        private GemonitordeItemsRepository repository;
        private UnitOfWorkManager uowManager;

        public GemonitordeItemsManager()
        {

        }

        public void AddGemonitordItem(GemonitordItem gemonitordItem)
        {
            InitNonExistingRepo();
            repository.CreateGemonitordItem(gemonitordItem);
        }
        public Persoon GetPersoon(int id, bool organisatie)
        {
            InitNonExistingRepo();
            return repository.ReadPersoon(id, organisatie);
        }

        public IEnumerable<GemonitordItem> GetGemonitordeItems(int deelplatformId)
        {
            InitNonExistingRepo();
            return repository.ReadGemonitordeItems().Where(a => a.DeelplatformId == deelplatformId);
        }

        public IEnumerable<GemonitordItem> GetPersonen(int deelplatformId)
        {
            InitNonExistingRepo();
            return repository.ReadGemonitordeItems().Where(a => a is Persoon && a.DeelplatformId == deelplatformId);
        }

        public IEnumerable<Persoon> GetPersonen(int deelplatformId, List<string> personen)
        {
            InitNonExistingRepo();
            List<Persoon> gemonitordeItems = new List<Persoon>();
            var allePersonen = repository.ReadGemonitordeItems().Where(a => a is Persoon && a.DeelplatformId == deelplatformId);
            foreach (var persoon in personen)
            {
                gemonitordeItems.Add(allePersonen.FirstOrDefault(a => a.Naam.Equals(persoon)) as Persoon);
            }
            return gemonitordeItems;


        }

        public IEnumerable<GemonitordItem> GetThemas(int deelplatformId)
        {
            InitNonExistingRepo();
            //return repository.ReadGemonitordeItems().Where(a => a is Thema && a.DeelplatformId == deelplatformId).Cast<Thema>();
            return repository.ReadGemonitordeItems().Where(a => a is Thema && a.DeelplatformId == deelplatformId);

        }


        public IEnumerable<GemonitordItem> GetOrganisaties(int deelplatformId)
        {
            InitNonExistingRepo();
            return repository.ReadGemonitordeItems().Where(a => a is Organisatie && a.DeelplatformId == deelplatformId);
        }

        //public IEnumerable<GemonitordItem> GetThemas(int deelplatformId, bool grafieken = false)
        //{
        //  InitNonExistingRepo();
        //  return repository.ReadGemonitordeItems(grafieken).Where(a => a is Thema && a.DeelplatformId == deelplatformId);
        //}


        public GemonitordItem GetGemonitordItem(int id)
        {
            InitNonExistingRepo();
            return repository.ReadGemonitordItem(id);
        }
        public GemonitordItem GetGemonitordItem(int deelplatformId, string naam, bool grafieken = false)
        {
            InitNonExistingRepo();
            return repository.ReadGemonitordeItems().FirstOrDefault(a => a.Naam.Equals(naam, StringComparison.OrdinalIgnoreCase) && a.DeelplatformId == deelplatformId);
        }

        public void ChangeGemonitordItem(GemonitordItem gemonitordItem)
        {
            InitNonExistingRepo();
            repository.UpdateGemonitordItem(gemonitordItem);
        }
        public void RemoveGemonitordeItems(IEnumerable<GemonitordItem> gemonitordeItems)
        {
            InitNonExistingRepo(true);
            repository.DeleteGemonitordeItems(gemonitordeItems);
        }
        public void RemoveGemonitordItem(GemonitordItem gemonitordItem)
        {
            InitNonExistingRepo();
            repository.DeleteGemonitordItem(gemonitordItem);
        }
        public void RemoveGemonitordItem(int id)
        {
            InitNonExistingRepo();
            repository.DeleteGemonitordItem(GetGemonitordItem(id));
        }
        public void AddThema(string naam, List<string> kernwoorden, int deelplatformId, bool volgbaar = true)
        {
            InitNonExistingRepo();
            if (GetThemas(deelplatformId).FirstOrDefault(a => a.Naam.Equals(naam, StringComparison.OrdinalIgnoreCase)) == null)
            {
                repository.CreateGemonitordItem(new Thema()
                {
                    Naam = naam,
                    KernWoorden = kernwoorden,
                    Volgbaar = true,
                    TotaalAantalVermeldingen = 0,
                    DeelplatformId = deelplatformId
                });
            }
        }

        public void AddPersoon(string naam, int deelplatformId, bool volgbaar = true)
        {
            InitNonExistingRepo();
            if (GetPersonen(deelplatformId).Where(a => a.Naam.Equals(naam, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() == null)
            {
                AddGemonitordItem(new Persoon() { Naam = naam, Volgbaar = volgbaar, DeelplatformId = deelplatformId });
            }
        }

        public void AddOrganisatie(string naam, int deelplatformId, List<string> namenPersonen)
        {
            InitNonExistingRepo();
            List<Persoon> toeTeVoegenPersonen = new List<Persoon>();
            if (GetOrganisaties(deelplatformId).FirstOrDefault(a => a.Naam.Equals(naam, StringComparison.OrdinalIgnoreCase)) == null)
            {
                Organisatie organisatie = new Organisatie() { Naam = naam, Volgbaar = true, TotaalAantalVermeldingen = 0, DeelplatformId = deelplatformId };
                List<GemonitordItem> bestaandePersonen = GetPersonen(deelplatformId).ToList();
                foreach (string persoon in namenPersonen)
                {
                    GemonitordItem toeTeVoegenPersoon = bestaandePersonen.FirstOrDefault(a => a.Naam.Equals(persoon, StringComparison.OrdinalIgnoreCase));
                    if (toeTeVoegenPersoon == null)
                    {
                        toeTeVoegenPersoon = new Persoon() { Naam = persoon, TotaalAantalVermeldingen = 0, DeelplatformId = deelplatformId };
                    }
                    toeTeVoegenPersonen.Add(toeTeVoegenPersoon as Persoon);
                }
                AddGemonitordItem(new Organisatie() { Naam = naam, Volgbaar = true, Personen = toeTeVoegenPersonen, DeelplatformId = deelplatformId });
            }
        }

        public void EditOrganisatie(int id, int deelplatformId, string naam, List<string> namenPersonen)
        {
            InitNonExistingRepo();
            List<Persoon> toeTeVoegenPersonen = new List<Persoon>();

            Organisatie organisatie = GetGemonitordItem(id) as Organisatie;
            List<GemonitordItem> bestaandePersonen = GetPersonen(deelplatformId).ToList();
            foreach (string persoon in namenPersonen)
            {
                GemonitordItem toeTeVoegenPersoon = bestaandePersonen.FirstOrDefault(a => a.Naam.Equals(persoon.Trim(), StringComparison.OrdinalIgnoreCase));
                if (toeTeVoegenPersoon == null)
                {
                    toeTeVoegenPersoon = new Persoon() { Naam = persoon, TotaalAantalVermeldingen = 0, DeelplatformId = deelplatformId };
                }
                toeTeVoegenPersonen.Add(toeTeVoegenPersoon as Persoon);
            }
            organisatie.Naam = naam;
            organisatie.Personen = toeTeVoegenPersonen;
            organisatie.BerekenEigenschappen();
            ChangeGemonitordItem(organisatie);

        }

        public void AddGekruistItem(GemonitordItem item1, GemonitordItem item2, string naam, int deelplatformId)
        {
            InitNonExistingRepo();
            GemonitordItem gekruistItem = new GekruistItem { Naam = naam, DeelplatformId = deelplatformId, Item1 = item1, Item2 = item2 };
            gekruistItem.BerekenEigenschappen();
            repository.CreateGemonitordItem(gekruistItem);
        }

        public void RefreshItems(DateTime syncDatum, int aantalDagenHistoriek, int deelplatformId)
        {
            InitNonExistingRepo();
            VoegDetailItemToeAanThemas(deelplatformId);
            VerwijderOudeDetailItems(syncDatum.AddDays(-aantalDagenHistoriek), deelplatformId);
            foreach (var item in repository.ReadGemonitordeItems().Where(a => a.DeelplatformId == deelplatformId).ToList())
            {
                item.BerekenEigenschappen();
                repository.UpdateGemonitordItem(item);
                MaakHistorieken(item, aantalDagenHistoriek, syncDatum);
            }
            AlertManager alertManager = new AlertManager();
            alertManager.GenereerAlerts();
        }

        public void VoegDetailItemToeAanThemas(int deelplatformId)
        {
            foreach (var thema in GetThemas(deelplatformId).ToList())
            {
                foreach (var detailitem in repository.ReadDetailItems())
                {
                    if (detailitem.Themas.Contains(thema.Naam))
                    {
                        thema.DetailItems.Add(detailitem);
                    }
                }
                ChangeGemonitordItem(thema);
            }
        }

        public Organisatie GetOrganisatie(string naamOrganisatie)
        {
            InitNonExistingRepo();
            return repository.ReadGemonitordeItems().Where(a => a.Naam.Equals(naamOrganisatie)).FirstOrDefault() as Organisatie;
        }

        private void VerwijderOudeDetailItems(DateTime limietDatum, int deelplatformId)
        {
            repository.DeleteDetailItems(limietDatum, deelplatformId);
        }


        private void MaakHistorieken(GemonitordItem item, int aantalDagenHistoriek, DateTime syncDatum)
        {
            DateTime startDatum;
            if (item.ItemHistorieken == null || item.ItemHistorieken.Count < 1 ||
                item.ItemHistorieken.OrderByDescending(a => a.HistoriekDatum).FirstOrDefault().HistoriekDatum < syncDatum.AddDays(-aantalDagenHistoriek))
            {
                startDatum = syncDatum.AddDays(-aantalDagenHistoriek);
            }
            else
            {
                startDatum = item.ItemHistorieken.OrderByDescending(a => a.HistoriekDatum).FirstOrDefault().HistoriekDatum;
            }
            DateTime startUur = startDatum;
            while (startUur < syncDatum)
            {
                DateTime grensUur = startUur.AddDays(1);
                //DateTime grensUur = startUur.AddHours(1);
                List<DetailItem> relevanteDetailItems = item.DetailItems.Where(a => a.BerichtDatum > startUur && a.BerichtDatum < grensUur).ToList();
                if (relevanteDetailItems.Count > 0)
                {
                    item.ItemHistorieken.Add(new ItemHistoriek()
                    {
                        HistoriekDatum = startUur,
                        AantalVermeldingen = relevanteDetailItems.Count,
                        AantalBerichtenVanMannen = relevanteDetailItems.Where(a => a.ProfielEigenschappen["gender"].Equals("m")).Count(),
                        AantalBerichtenVanVrouwen = relevanteDetailItems.Where(a => a.ProfielEigenschappen["gender"].Equals("f")).Count(),
                        GemObjectiviteit = relevanteDetailItems.Average(a => a.Objectiviteit),
                        GemPolariteit = relevanteDetailItems.Average(a => a.Polariteit),
                    });
                }
                else
                {
                    item.ItemHistorieken.Add(new ItemHistoriek()
                    {
                        HistoriekDatum = startUur,
                        AantalVermeldingen = 0,
                        AantalBerichtenVanMannen = 0,
                        AantalBerichtenVanVrouwen = 0,
                        GemObjectiviteit = 0.5,
                        GemPolariteit = 0
                    });
                }

                startUur = grensUur;

            }
            item.ItemHistorieken.RemoveAll(a => a.HistoriekDatum < startDatum);

            ChangeGemonitordItem(item);
        }

        public void InitNonExistingRepo(bool uow = false)
        {
            if (uow)
            {
                if (uowManager == null)
                {
                    uowManager = new UnitOfWorkManager();
                    repository = new GemonitordeItemsRepository(uowManager.UnitOfWork);
                }
            }
            else
            {
                repository = repository ?? new GemonitordeItemsRepository();
            }
        }
    }
}

