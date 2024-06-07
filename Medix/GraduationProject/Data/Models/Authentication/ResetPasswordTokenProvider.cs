using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace GraduationProject.Data.Models.Authentication
{
    public class ResetPasswordTokenProvider: TotpSecurityStampBasedTokenProvider<AppUser>
    {
        public const string ProviderKey = "ResetPassword";
       
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<AppUser> manager, AppUser user)
        {
            return Task.FromResult(false);
        }
    }
}
