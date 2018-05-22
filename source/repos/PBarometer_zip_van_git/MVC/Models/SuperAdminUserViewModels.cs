using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
  public class SuperAdminUserViewModel
  {
    [Display(Name = "Admin?")]
    public bool IsAdmin { get; set; }

    [Display(Name = "Voornaam")]
    public string FirstName { get; set; }

    [Display(Name = "Achternaam")]
    public string LastName { get; set; }

    [Display(Name = "E-mailadres")]
    public string Email { get; set; }
  }

  public class GebruikersViewModel
  {
    [Display(Name = "Voornaam")]
    public string FirstName { get; set; }

    [Display(Name = "Achternaam")]
    public string LastName { get; set; }

    [Display(Name = "E-mailadres")]
    public string Email { get; set; }
  }
}