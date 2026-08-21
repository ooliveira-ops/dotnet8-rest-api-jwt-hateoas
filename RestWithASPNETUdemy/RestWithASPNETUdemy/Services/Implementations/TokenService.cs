using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestWithASPNETUdemy.Configurations;

namespace RestWithASPNETUdemy.Services.Implementations
{
	public class TokenService : ITokenService
	{

		private TokenConfiguration _configuration;

		// Construtor que recebe a configuração do token.
		public TokenService(TokenConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateAccessToken(IEnumerable<Claim> claims)
		{
			// Cria uma chave simétrica a partir da secret configurado.
			var secretKet = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));

			// Cria as credenciais de assinatura usando a chave simétrica e o algoritmo HMAC SHA256.
			var siginingCredentials = new SigningCredentials(secretKet, SecurityAlgorithms.HmacSha256);

			// Cria o token JWT com as informações fornecidas.
			var options = new JwtSecurityToken(
				issuer: _configuration.Issuer,
				audience: _configuration.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.Minutes)),
				signingCredentials: siginingCredentials
			);
			// Gera a string do token JWT a partir das opções do token e retorna a string.
			string tokenString = new JwtSecurityTokenHandler().WriteToken(options);
			return tokenString;
		}

		// Gera um token de atualização (refresh token) aleatório.
		public string GenerateRefreshToken()
		{
			// Gera um array de bytes aleatórios de 32 bytes usando o gerador de números aleatórios criptograficamente seguro.
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			};
		}

		// Obtém as reivindicações do token expirado.
		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			// Cria um validador de token JWT para validar o token fornecido.
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret)),
				ValidateLifetime = false
			};
			// Esses parâmetros são usados para validar o token JWT.
			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;

			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;

			if (jwtSecurityToken == null || 
			!jwtSecurityToken.Header.Alg.Equals(
			SecurityAlgorithms.HmacSha256, 
			StringComparison.InvariantCultureIgnoreCase))
			throw new SecurityTokenException("Invalid token");

			return principal;
		}
	}
}
