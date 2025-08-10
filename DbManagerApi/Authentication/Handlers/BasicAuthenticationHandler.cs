using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace DbManagerApi.Authentication.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly SpellTestDbContext _context;

        private enum ContactType
        {
            Unknown,
            Email,
            Phone
        }

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder,
            SpellTestDbContext dbContext)
            : base(options, logger, encoder)
        {
            _context = dbContext;
        }

        
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization header");
            }

            string headerValue = Request.Headers["Authorization"].ToString();

            if (!AuthenticationHeaderValue.TryParse(headerValue, out AuthenticationHeaderValue? parsedValue))
            {
                return AuthenticateResult.Fail("Invalid Authorization value");
            }

            if (!"Basic".Equals(parsedValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Invalid Authorization scheme");
            }

            byte[] base64 = Convert.FromBase64String(parsedValue.Parameter!);
            string[] credentials = Encoding.UTF8.GetString(base64).Split(':', 2);

            if (credentials.Length != 2)
            {
                return AuthenticateResult.Fail("Invalid Authorization header");
            }

            string login = credentials[0];
            string password = credentials[1];

            var user = (GetContactType(login)) switch
            {
                ContactType.Email => await _context.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Email == login),
                ContactType.Phone => await _context.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Number == login),
                _ => null
            };
            if (user == null || user.Password != password)
            {
                return AuthenticateResult.Fail("Invalid sign in data");
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, login)
            };

            foreach (Role role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);

            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            AuthenticationTicket authenticationTicket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(authenticationTicket);
        }

        private ContactType GetContactType(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ContactType.Unknown;

            if (IsValidEmail(input))
                return ContactType.Email;

            if (IsValidPhone(input))
                return ContactType.Phone;

            return ContactType.Unknown;
        }

        private bool IsValidPhone(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            string pattern = @"^\+?[0-9]{7,15}$";
            return Regex.IsMatch(input, pattern);
        }

        private bool IsValidEmail(string input)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(input);
        }
    }
}
