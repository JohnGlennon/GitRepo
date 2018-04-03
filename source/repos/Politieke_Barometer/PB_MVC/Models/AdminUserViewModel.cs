using System.ComponentModel.DataAnnotations;

namespace PB_MVC.Models
{
  public class AdminUserViewModel
  {
    [Display(Name = "E-mailadres")]
    public string Email { get; set; }
  }
}