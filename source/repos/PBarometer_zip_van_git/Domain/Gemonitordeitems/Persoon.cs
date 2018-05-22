using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Gemonitordeitems
{
    public class Persoon : GemonitordItem
    {
        public string TwitterHandle { get; set; }
        public string Website { get; set; }
        public DateTime? Geboortedatum { get; set; }
        public string Facebook { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }
        public Organisatie Organisatie { get; set; }
        public int? OrganisatieId { get; set; }
    }
}
