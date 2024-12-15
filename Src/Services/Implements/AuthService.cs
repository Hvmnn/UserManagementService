using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using UserManagementService.Src.Services.Interfaces;

namespace UserManagementService.Src.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IHttpContextAccessor httpContextAccessor, ILogger<AuthService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string GetUserEmailInToken()
        {
            var httpUser = GetHttpUser();

            foreach (var claim in httpUser.Claims)
            {
                _logger.LogInformation($"Claim: {claim.Type} - {claim.Value}");
            }

            var userEmail = httpUser.FindFirstValue(ClaimTypes.Email) 
                            ?? httpUser.FindFirstValue("email")
                            ?? throw new UnauthorizedAccessException("Invalid email in token");

            _logger.LogInformation($"Email extraído del token: {userEmail}");
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
            var httpUser = _httpContextAccessor.HttpContext?.User;

            if (httpUser == null)
            {
                Console.WriteLine("El contexto HTTP es nulo.");
                throw new UnauthorizedAccessException("Usuario no autenticado.");
            }

            if (!httpUser.Identity.IsAuthenticated)
            {
                Console.WriteLine("El usuario no está autenticado.");
                throw new UnauthorizedAccessException("Usuario no autenticado.");
            }

            Console.WriteLine("Usuario autenticado. Claims disponibles:");
            foreach (var claim in httpUser.Claims)
            {
                Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
            }

            return httpUser;
        }
    }
}