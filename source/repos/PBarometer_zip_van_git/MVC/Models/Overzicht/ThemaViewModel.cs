using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models.Overzicht
{
    public class ThemaViewModel
    {
        [Required]
        public string Naam { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Kernwoorden { get; set; }
        public int Id { get; set; }
    }
}