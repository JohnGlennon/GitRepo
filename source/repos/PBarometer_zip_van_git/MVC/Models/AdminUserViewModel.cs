using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
  public class AdminUserViewModel
  {
    [Display(Name = "E-mailadres")]
    public string Email { get; set; }
  }
}