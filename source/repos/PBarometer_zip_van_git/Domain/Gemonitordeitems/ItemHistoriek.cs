using System;

namespace Domain.Gemonitordeitems
{
    public class ItemHistoriek
    {
        public DateTime HistoriekDatum { get; set; }
        public int AantalVermeldingen { get; set; }
        public double GemPolariteit { get; set; }
        public double GemObjectiviteit { get; set; }
        public int ItemHistoriekId { get; set; }
        public int AantalBerichtenVanMannen { get; set; }
        public int AantalBerichtenVanVrouwen { get; set; }
        public ItemHistoriek()
        {

        }
    }
}