using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models.Android
{
    public class AlertDTO
    {
        public bool Triggered { get; set; }
        public string Beschrijving { get; set; }
        public string Onderwerp { get; set; }
        public bool Geactiveerd { get; set; }
        public int Id { get; set; }
    }
}