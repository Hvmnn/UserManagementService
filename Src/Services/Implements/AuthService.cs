using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using UserManagementService.Src.Services.Interfaces;

namespace UserManagementService.Src.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserEmailInToken()
        {
            var httpUser = GetHttpUser();
            var userEmail = httpUser.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException("Invalid email in token");
            return userEmail;
        }

        public string GetUserRoleInToken()
        {
            var httpUser = GetHttpUser();
            var userRole = httpUser.FindFirstValue(ClaimTypes.Role) ?? throw new UnauthorizedAccessException("Invalid role in token");
            return userRole;
        }

        private ClaimsPrincipal GetHttpUser()
        {
            return (_httpContextAccessor.HttpContext?.User) ?? throw new UnauthorizedAccessException();
        }
    }
}