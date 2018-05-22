using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models.Overzicht
{
    public class PersoonViewModel
    {
        [Required]
        public string Naam { get; set; }
        public DateTime? Geboortedatum { get; set; }
        [Display(Name = "Twitter")]
        public string TwitterHandle { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }
        [Display(Name = "Organisatie")]
        public string NaamOrganisatie { get; set; }
        public int Id { get; set; }
    }
}