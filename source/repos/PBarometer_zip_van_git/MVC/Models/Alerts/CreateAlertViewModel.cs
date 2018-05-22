using Domain.Gemonitordeitems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models.Alerts
{
    public class CreateAlertViewModel
    {
        public int Id { get; set; }
        public string Onderwerp { get; set; }
        [Required]
        public string Beschrijving { get; set; }
        public bool Mail { get; set; }
        public bool Mobiel { get; set; }
        [Display(Name = "Minimum daling")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public double MinDaling { get; set; }
        [Display(Name = "Minimum dalingsperiode")]
        public double MinDalingPeriode { get; set; }
        [Display(Name = "Minimum stijging")]
        public double MinStijging { get; set; }
        [Display(Name = "Minimum stijgingsperiode")]
        public double MinStijgingPeriode { get; set; }
        [Display(Name = "Niet belangrijk waarde")]
        public double NietBelangrijkWaarde { get; set; }
        [Display(Name = "Belangrijk waarde")]
        public double BelangrijkWaarde { get; set; }
        [Display(Name = "Belangrijkheidsperiode")]
        public double BelangrijkheidsPeriode { get; set; }
        [Display(Name = "Minimum polariteit")]
        public double MinPolariteit { get; set; }
        [Display(Name = "Maximum polariteit")]
        public double MaxPolariteit { get; set; }
        [Display(Name = "Polariteitsperiode")]
        public double PolariteitsPeriode { get; set; }
        [Display(Name = "Minimum objectivieit")]
        public double MinObjectiviteit { get; set; }
        [Display(Name = "Maximum objectiviteit")]
        public double MaxObjectiviteit { get; set; }
        [Display(Name = "Objectiviteitsperiode")]
        public double ObjectiviteitsPeriode { get; set; }
    }
}