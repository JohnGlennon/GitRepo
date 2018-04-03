using DAL.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IdentityFramework
{
  public class ApplicationRoleManager : RoleManager<IdentityRole>
  {

    public ApplicationRoleManager()
        : base(new RoleStore<IdentityRole>(new PBDbContext()))
    {
    }

    public static ApplicationRoleManager Create()
    {
      return new ApplicationRoleManager();
    }
  }
}
