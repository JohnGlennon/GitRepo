using System.ComponentModel.DataAnnotations;

namespace PB_MVC.Models
{
  public class SuperAdminUserViewModel
  {
    [Display(Name = "E-mailadres")]
    public string Email { get; set; }
  }
}