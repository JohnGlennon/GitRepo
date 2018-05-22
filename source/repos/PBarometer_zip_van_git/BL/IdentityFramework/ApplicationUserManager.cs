using DAL.EF;
using Domain.Dashboards;
using Domain.Deelplatformen;
using Domain.IdentityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.IdentityFramework
{

  // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
  public class ApplicationUserManager : UserManager<ApplicationUser>
  {
    private DeelplatformenManager deelplatformenManager;

    public ApplicationUserManager() : base(new UserStore<ApplicationUser>(new DbContext()))
    {
      deelplatformenManager = new DeelplatformenManager();
      CreateFirstSuperAdmin();
      //CreateUsers();
    }

    private void CreateFirstSuperAdmin()
    {
      //Create User=superadmin@example.com with password=SuperAdmin@123456 in the SuperAdmin role        

      List<Deelplatform> deelplatformen = deelplatformenManager.GetDeelplatformen().ToList();
      List<Dashboard> dashboards = new List<Dashboard>();

      foreach (Deelplatform deelplatform in deelplatformen)
      {
        dashboards.Add(new Dashboard() { DeelplatformId = deelplatform.DeelplatformId });
      }

      var roleManager = new ApplicationRoleManager();
      const string name = "superadmin@example.com";
      const string password = "SuperAdmin@123456";
      const string roleName1 = "SuperAdmin";
      const string roleName2 = "Admin";

      //Create Role superadmin if it does not exist
      var role1 = roleManager.FindByName(roleName1);
      if (role1 == null)
      {
        role1 = new IdentityRole(roleName1);
        var roleresult1 = roleManager.Create(role1);
      }

      var role2 = roleManager.FindByName(roleName2);
      if (role2 == null)
      {
        role2 = new IdentityRole(roleName2);
        var roleresult2 = roleManager.Create(role2);
      }

      var user = this.FindByName(name);
      if (user == null)
      {
        user = new ApplicationUser { UserName = name, Email = name, Dashboards = dashboards };
        var result = this.Create(user, password);
        result = this.SetLockoutEnabled(user.Id, false);
      }

      // Add user superadmin to Role SuperAdmin if not already added
      var rolesForUser = this.GetRoles(user.Id);
      if (!rolesForUser.Contains(role1.Name))
      {
        var result = this.AddToRole(user.Id, role1.Name);
      }

      if (!rolesForUser.Contains(role2.Name))
      {
        var result = this.AddToRole(user.Id, role2.Name);
      }
    }

    //private void CreateUsers()
    //{
    //  List<Deelplatform> deelplatformen = deelplatformenManager.GetDeelplatformen().ToList();
    //  List<Dashboard> dashboards = new List<Dashboard>();

    //  foreach (Deelplatform deelplatform in deelplatformen)
    //  {
    //    dashboards.Add(new Dashboard() { DeelplatformId = deelplatform.DeelplatformId });
    //  }

    //  var roleManager = new ApplicationRoleManager();
    //  string name = "jelle@example.com";
    //  string password = "Jelle@123456";
    //  string rolename = "Gebruiker";

    //  var role = roleManager.FindByName(rolename);
    //  if (role == null)
    //  {
    //    role = new IdentityRole(rolename);
    //    roleManager.Create(role);
    //  }

    //  var user = this.FindByName(name);
    //  if (user == null)
    //  {
    //    user = new ApplicationUser { UserName = name, Email = name, Dashboards = dashboards };
    //    var result = this.Create(user, password);
    //    result = this.SetLockoutEnabled(user.Id, false);
    //  }
    //  var rolesForUser = this.GetRoles(user.Id);
    //  if (!rolesForUser.Contains(role.Name))
    //  {
    //    var result = this.AddToRole(user.Id, role.Name);
    //  }

    //  dashboards = new List<Dashboard>();
    //  foreach (Deelplatform deelplatform in deelplatformen)
    //  {
    //    dashboards.Add(new Dashboard() { DeelplatformId = deelplatform.DeelplatformId });
    //  }

    //  name = "bart@example.com";
    //  password = "Bart@123456";
    //  rolename = "Gebruiker";

    //  role = roleManager.FindByName(rolename);
    //  if (role == null)
    //  {
    //    role = new IdentityRole(rolename);
    //    roleManager.Create(role);
    //  }

    //  user = this.FindByName(name);
    //  if (user == null)
    //  {
    //    user = new ApplicationUser { UserName = name, Email = name, Dashboards = dashboards };
    //    var result = this.Create(user, password);
    //    result = this.SetLockoutEnabled(user.Id, false);
    //  }
    //  rolesForUser = this.GetRoles(user.Id);
    //  if (!rolesForUser.Contains(role.Name))
    //  {
    //    var result = this.AddToRole(user.Id, role.Name);
    //  }
    //}

    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
    {
      var manager = new ApplicationUserManager();
      // Configure validation logic for usernames
      manager.UserValidator = new UserValidator<ApplicationUser>(manager)
      {
        AllowOnlyAlphanumericUserNames = false,
        RequireUniqueEmail = true
      };

      // Configure validation logic for passwords
      manager.PasswordValidator = new PasswordValidator
      {
        RequiredLength = 6,
        RequireNonLetterOrDigit = false,
        RequireDigit = true,
        RequireLowercase = true,
        RequireUppercase = true,
      };

      // Configure user lockout defaults
      manager.UserLockoutEnabledByDefault = true;
      manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
      manager.MaxFailedAccessAttemptsBeforeLockout = 5;

      manager.EmailService = new EmailService();
      var dataProtectionProvider = options.DataProtectionProvider;
      if (dataProtectionProvider != null)
      {
        manager.UserTokenProvider =
            new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
      }
      return manager;
    }

    public List<ApplicationUser> GetRegularUsersAndAdmins(List<ApplicationUser> users)
    {
      List<ApplicationUser> regularUsersAndAdmins = new List<ApplicationUser>();
      foreach (ApplicationUser user in users)
      {
        bool isRegularUserOrAdmin = true;
        var roles = this.GetRoles(user.Id);
        foreach (string role in roles)
        {
          if (role.Equals("SuperAdmin"))
          {
            isRegularUserOrAdmin = false;
          }
        }
        if (isRegularUserOrAdmin)
        {
          regularUsersAndAdmins.Add(user);
        }
      }
      return regularUsersAndAdmins;
    }

    public List<ApplicationUser> GetAdmins(List<ApplicationUser> users)
    {
      List<ApplicationUser> admins = new List<ApplicationUser>();

      foreach (ApplicationUser user in users)
      {
        var roles = this.GetRoles(user.Id);
        bool isAdmin = false;
        bool isSuperAdmin = false;
        foreach (string role in roles)
        {
          if (role.Equals("Admin"))
          {
            isAdmin = true;
          }
          if (role.Equals("SuperAdmin"))
          {
            isSuperAdmin = true;
          }
        }
        if (isAdmin && !isSuperAdmin)
        {
          admins.Add(user);
        }
      }

      return admins;
    }
  }
}