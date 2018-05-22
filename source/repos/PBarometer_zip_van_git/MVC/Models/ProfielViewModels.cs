using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
  public class IndexViewModel
  {
    public IList<UserLoginInfo> Logins { get; set; }
    public bool BrowserRemembered { get; set; }
  }

  public class ChangeNameViewModel
  {
    [Required]
    [Display(Name = "Nieuwe voornaam")]
    public string NewFirstName { get; set; }

    [Required]
    [Display(Name = "Nieuwe achternaam")]
    public string NewLastName { get; set; }
  }

  public class ChangeEmailViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Huidig emailadres")]
    public string OldEmail { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Nieuw emailadres")]
    public string NewEmail { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Nieuw emailadres bevestigen")]
    [Compare("NewEmail", ErrorMessage = "Het nieuwe emailadres en emailadresbevestiging komen niet overeen.")]
    public string ConfirmEmail { get; set; }
  }

  public class ChangePasswordViewModel
  {
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Huidig wachtwoord")]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Het wachtwoord moet minstens {2} karakters lang zijn.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Nieuw wachtwoord")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Nieuw wachtwoord bevestigen")]
    [Compare("NewPassword", ErrorMessage = "Het nieuwe wachtwoord en wachtwoordbevestiging komen niet overeen.")]
    public string ConfirmPassword { get; set; }
  }
}