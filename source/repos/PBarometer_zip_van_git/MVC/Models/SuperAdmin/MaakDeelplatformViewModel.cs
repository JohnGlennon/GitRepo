using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models.SuperAdmin
{
    public class MaakDeelplatformViewModel
    {
        [Required]
        public string Naam { get; set; }
        [Required]
        [Display(Name = "URL")]
        public string URLNaam { get; set; }
        [Required]
        [Display(Name = "Aantal dagen historiek")]
        public int AantalDagenHistoriek { get; set; }
        public HttpPostedFileBase Afbeelding { get; set; }
        public int Id { get; set; }
    }
}