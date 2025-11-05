using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenCRM.Core.Data;
using OpenCRM.Core.Web.Areas.Identity.Models;
using OpenCRM.Core.Web.Models;

namespace OpenCRM.Core.Web.Services.IdentityService
{
    public interface IIdentityService
    {
        Task<IdentityResult> ConfirmUserEmail(UserEntity user, string token);
        Task<UserModel?> GetLoggedUser();
        DataSession? GetSession();
        Task<UserEntity?> GetUser(string userId);
        Task<UserEntity?> GetUserByUsername(string username);
        Task Logout();
        Task<Tuple<IdentityResult, UserEntity>> RegisterUser(InputRegisterModel Input);
        Task Seed();
        Task<bool> SendConfirmationEmail(UserEntity user, PageModel page);
        void SetSession(DataSession userSession);
        Task<SignInResult> SignInUser(string username, string password, bool rememberMe);
    }
}