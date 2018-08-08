using Microsoft.AspNet.Identity;
using OEP.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OEP.Web.Helpers
{
    public static class ClaimManagement
    {
        public static async Task ManageClaimsAfterDbUpdate(ApplicationUser userprofile, ClaimsIdentity identity, ApplicationUserManager userManager)
        {           

            // Remove the existing claim value of current user from database
            if (identity.FindFirst("NameOfUser") != null)
                await userManager.RemoveClaimAsync(userprofile.Id, identity.FindFirst("NameOfUser"));
            if (identity.FindFirst("ProfilePicture") != null)
                await userManager.RemoveClaimAsync(userprofile.Id, identity.FindFirst("ProfilePicture"));
            if (identity.FindFirst("PackageId") != null)
                await userManager.RemoveClaimAsync(userprofile.Id, identity.FindFirst("PackageId"));

            // Update customized claim 
            await userManager.AddClaimAsync(userprofile.Id, new Claim("NameOfUser", userprofile.Name));
            await userManager.AddClaimAsync(userprofile.Id, new Claim("ProfilePicture", !string.IsNullOrEmpty(userprofile.ProfilePicture) ? userprofile.ProfilePicture : ""));
            await userManager.AddClaimAsync(userprofile.Id, new Claim("PackageId", userprofile.PackageId.ToString()));
        }
    }
}