using DAL.EF;
using Domain.IdentityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;

namespace BL.IdentityFramework
{
  // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
  public class ApplicationUserManager : UserManager<ApplicationUser>
  {
    public ApplicationUserManager() : base(new UserStore<ApplicationUser>(new PBDbContext()))
    {
      CreateFirstAdmin();
      CreateFirstSuperAdmin();
    }

    private void CreateFirstAdmin()
    {
      //Create User=admin@example.com with password=Admin@123456 in the Admin role        

      var roleManager = new ApplicationRoleManager();
      const string name = "admin@example.com";
      const string password = "Admin@123456";
      const string roleName = "Admin";

      //Create Role Admin if it does not exist
      var role = roleManager.FindByName(roleName);
      if (role == null)
      {
        role = new IdentityRole(roleName);
        var roleresult = roleManager.Create(role);
      }

      var user = this.FindByName(name);
      if (user == null)
      {
        user = new ApplicationUser { UserName = name, Email = name };
        var result = this.Create(user, password);
        result = this.SetLockoutEnabled(user.Id, false);
      }

      // Add user admin to Role Admin if not already added
      var rolesForUser = this.GetRoles(user.Id);
      if (!rolesForUser.Contains(role.Name))
      {
        var result = this.AddToRole(user.Id, role.Name);
      }
    }

    private void CreateFirstSuperAdmin()
    {
      //Create User=superadmin@example.com with password=SuperAdmin@123456 in the SuperAdmin role        

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
        user = new ApplicationUser { UserName = name, Email = name };
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
        RequireNonLetterOrDigit = true,
        RequireDigit = true,
        RequireLowercase = true,
        RequireUppercase = true,
      };

      // Configure user lockout defaults
      manager.UserLockoutEnabledByDefault = true;
      manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
      manager.MaxFailedAccessAttemptsBeforeLockout = 5;

      // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
      // You can write your own provider and plug it in here.
      manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
      {
        Subject = "Security Code",
        BodyFormat = "Your security code is {0}"
      });
      manager.EmailService = new EmailService();
      var dataProtectionProvider = options.DataProtectionProvider;
      if (dataProtectionProvider != null)
      {
        manager.UserTokenProvider =
            new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
      }
      return manager;
    }
  }
}
