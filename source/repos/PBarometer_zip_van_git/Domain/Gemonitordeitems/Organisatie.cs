using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Gemonitordeitems
{
    public class Organisatie : GemonitordItem
    {
        public Organisatie()
        {
            Personen = new List<Persoon>();
        }
        public List<Persoon> Personen { get; set; }

        public override void BerekenEigenschappen()
        {
            List<DetailItem> detailitems = new List<DetailItem>();
            foreach (var persoon in Personen)
            {
                detailitems = detailitems.Concat(persoon.DetailItems).ToList();
            }
            DetailItems = detailitems;
            base.BerekenEigenschappen();
        }
    }
}
