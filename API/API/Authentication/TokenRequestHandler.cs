using API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace API.Authentication
{
    public class TokenRequestHandler : IOpenIddictServerHandler<HandleTokenRequestContext>
    {
        private readonly IRepository _repo;

        public TokenRequestHandler(IRepository usuariosRepository)
        {
            _repo = usuariosRepository;
        }

        public async ValueTask HandleAsync(HandleTokenRequestContext context)
        {
            ClaimsIdentity? identity;
            if (context.Request.IsPasswordGrantType())
            {
                identity = await HandlePasswordCredentialsAsync(context);
            }
            else if (context.Request.IsRefreshTokenGrantType())
            {
                identity = await HandleRefreshTokenAsync(context);
            }
            else
            {
                throw new InvalidOperationException("The specified grant type is not supported.");
            }

            if (identity != null)
            {
                var principal = new ClaimsPrincipal(identity);
                principal.SetScopes(Scopes.OpenId, Scopes.OfflineAccess);
                context.SignIn(principal);
            }
        }

        private async ValueTask<ClaimsIdentity?> HandlePasswordCredentialsAsync(HandleTokenRequestContext context)
        {
            var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType);

            if (string.IsNullOrEmpty(context.Request.Username) || string.IsNullOrEmpty(context.Request.Password))
            {
                context.Reject(Errors.InvalidGrant, "O usuário ou a senha estão incorretos.");
                return null;
            }

            var usuario = await _repo.SingleOrDefaultByEmailAsync(context.Request.Username.ToLower());
            if (usuario == null)
            {
                context.Reject(Errors.InvalidGrant, "O usuário ou a senha estão incorretos.");
                return null;
            }

            if (usuario.Password != context.Request.Password)
            {
                context.Reject(Errors.InvalidGrant, "O usuário ou a senha estão incorretos.");
                return null;
            }

            identity.AddClaim(new Claim(Claims.Subject, usuario.Id.ToString()).SetDestinations(Destinations.AccessToken, Destinations.IdentityToken));
            identity.AddClaim(new Claim(Claims.Name, usuario.Name).SetDestinations(Destinations.AccessToken, Destinations.IdentityToken));
            identity.AddClaim(new Claim(Claims.Email, usuario.Email).SetDestinations(Destinations.AccessToken, Destinations.IdentityToken));

            return identity;
        }

        private ValueTask<ClaimsIdentity?> HandleRefreshTokenAsync(HandleTokenRequestContext context)
        {
            return new ValueTask<ClaimsIdentity?>(context.Principal?.Identities.FirstOrDefault());
        }
    }
}
