using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models.Alerts
{
    public class AlertViewModel
    {
        public int Id { get; set; }
        public string TriggerRedenen { get; set; }
        public bool Triggered { get; set; }
        public string Beschrijving { get; set; }
        public string Onderwerp { get; set; }
        public bool Geactiveerd { get; set; }
        public bool Mail { get; set; }
        public bool Mobiel { get; set; }
        public bool Eenvoudig { get; set; }
    }
}