using System.Web;

namespace MVC.Models
{
    public class LayoutViewModel
    {
        public string Kleur { get; set; }
        public HttpPostedFileBase Afbeelding { get; set; }
    }
}