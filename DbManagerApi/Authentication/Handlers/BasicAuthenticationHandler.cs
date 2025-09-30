using Application.Interfaces;
using DomainData.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
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
        private IUserService UserService { get; init; }

        private IPasswordHasher<User> PasswordHasher { get; init; }

        private enum ContactType
        {
            Unknown,
            Email,
            Phone
        }

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IUserService userService,
            IPasswordHasher<User> passwordHasher)
            : base(options, logger, encoder)
        {
            UserService = userService;
            PasswordHasher = passwordHasher;
        }
        //Sentree

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

            if (parsedValue.Parameter is null)
            {
                return AuthenticateResult.Fail("Invalid Authorization value");
            }

            byte[] base64;
            try
            {
                base64 = Convert.FromBase64String(parsedValue.Parameter);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
            string[] credentials = Encoding.UTF8.GetString(base64).Split(':', 2);

            if (credentials.Length != 2)
            {
                return AuthenticateResult.Fail("Invalid Authorization header");
            }

            string login = credentials[0];
            string password = credentials[1];

            var user = (GetContactType(login)) switch
            {
                ContactType.Email => await UserService.GetByEmailIncludeRolesAsync(login),
                ContactType.Phone => await UserService.GetByPhoneIncludeRolesAsync(login),
                _ => null
            };
            if (user == null || PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
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
