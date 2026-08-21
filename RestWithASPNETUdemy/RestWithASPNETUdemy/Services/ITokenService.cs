using System.Security.Claims;

namespace RestWithASPNETUdemy.Services
{
	public interface ITokenService
	{
		// Gera um token de acesso com base nas reivindicações fornecidas.
		string GenerateAccessToken(IEnumerable<Claim> claims);

		// Gera um token de atualização.
		string GenerateRefreshToken();

		// Obtém as reivindicações do token expirado.
		ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
	}
}
