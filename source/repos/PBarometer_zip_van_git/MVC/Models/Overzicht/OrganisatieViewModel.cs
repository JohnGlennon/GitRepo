using System.ComponentModel.DataAnnotations;

namespace MVC.Controllers
{
    public class OrganisatieViewModel
    {
        [Required]
        public string Naam { get; set; }
        [DataType(DataType.MultilineText)]
        public string Leden { get; set; }
        public int Id { get; set; }
    }
}