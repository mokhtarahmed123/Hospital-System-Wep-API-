using HospitalAPI.Hospital.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;

namespace HospitalAPI.Hospital.API.Middleware
{
    public class TokenRevocationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenRevocationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, HospitalContex dbContext)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var jti = jwtToken?.Claims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrEmpty(jti))
                {
                    var isRevoked = dbContext.revokedTokens.Any(rt => rt.JWT == jti);
                    if (isRevoked)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token has been revoked.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
