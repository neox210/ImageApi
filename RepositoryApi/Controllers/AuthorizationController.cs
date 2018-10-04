using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server;

namespace RepositoryApi.Controllers
{
    public class AuthorizationController : Controller
    {
        [HttpPost("~/connect/token"), Produces("application/json")]
        public IActionResult Exchange(OpenIdConnectRequest request)
        {
            if (!request.IsPasswordGrantType())
            {
                throw new InvalidOperationException("The specified grant type is not supported.");
            }
                
            // Validate the user credentials.
            // Note: to mitigate brute force attacks, you SHOULD strongly consider
            // applying a key derivation function like PBKDF2 to slow down
            // the password validation process. You SHOULD also consider
            // using a time-constant comparer to prevent timing attacks.
            if (request.Username != "alice@wonderland.com" ||
                request.Password != "P@ssw0rd")
            {
                return Forbid(OpenIddictServerDefaults.AuthenticationScheme);
            }
            // Create a new ClaimsIdentity holding the user identity.
            var identity = new ClaimsIdentity(
                OpenIddictServerDefaults.AuthenticationScheme,
                OpenIdConnectConstants.Claims.Name,
                OpenIdConnectConstants.Claims.Role);
            // Add a "sub" claim containing the user identifier, and attach
            // the "access_token" destination to allow OpenIddict to store it
            // in the access token, so it can be retrieved from your controllers.
            identity.AddClaim(OpenIdConnectConstants.Claims.Subject,
                "71346D62-9BA5-4B6D-9ECA-755574D628D8",
                OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim(OpenIdConnectConstants.Claims.Name, "Alice",
                OpenIdConnectConstants.Destinations.AccessToken);
            // ... add other claims, if necessary.
            var principal = new ClaimsPrincipal(identity);
            // Ask OpenIddict to generate a new token and return an OAuth2 token response.
            return SignIn(principal, OpenIddictServerDefaults.AuthenticationScheme);
        }
    }
}
