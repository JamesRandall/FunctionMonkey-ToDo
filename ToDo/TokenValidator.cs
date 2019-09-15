using System.Security.Claims;
using System.Threading.Tasks;
using FunctionMonkey.Abstractions;

namespace ToDo
{
    public class TokenValidator : ITokenValidator
    {
        public Task<ClaimsPrincipal> ValidateAsync(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Task.FromResult<ClaimsPrincipal>(null);
            }

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(
                new[]
                {
                    new ClaimsIdentity(new[]
                    {
                        new Claim("userId", "user1"),
                    }),
                }
            );
            return Task.FromResult(claimsPrincipal);
        }
    }
}