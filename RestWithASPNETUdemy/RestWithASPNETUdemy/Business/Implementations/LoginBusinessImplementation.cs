using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using RestWithASPNETUdemy.Configurations;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Repository;
using RestWithASPNETUdemy.Services;
using RestWithASPNETUdemy.Services.Implementations;

namespace RestWithASPNETUdemy.Business.Implementations
{
	// essa classe fornece a lógica de negócios para validar as credenciais do usuário e gerar tokens de acesso e atualização.
	public class LoginBusinessImplementation : ILoginBusiness
	{
		private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
		private TokenConfiguration _configuration;

		private IUserRepository _repository;
		private readonly ITokenService _tokenService;

		public LoginBusinessImplementation(TokenConfiguration configuration, IUserRepository repository, ITokenService tokenService)
		{
			_configuration = configuration;
			_repository = repository;
			_tokenService = tokenService;
		}

		public TokenVO ValidateCredentials(UserVO userCredentials)
		{
			// pega as credenciais do usuario e valida no banco de dados, se o usuário for válido, gera
			// os tokens de acesso e atualização e retorna um objeto TokenVO com as informações do token.
			var user = _repository.ValidateCredentials(userCredentials);
			if (user == null) return null;

			// Cria as 'reivindicações' do usuário
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
			};

			// Gerar tokens de acesso
			var accessToken = _tokenService.GenerateAccessToken(claims);
			// Gerar token de atualização(quando o token de acesso expirar)
			var refreshToken = _tokenService.GenerateRefreshToken();

			// Seta o access e o refresh no usuario (user) e atualiza no banco de dados
			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpiry);

			_repository.RefreshUserInfo(user);

			DateTime createDate = DateTime.Now;
			DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

			// Retorna o objeto TokenVO com as informações do token
			return new TokenVO(
				true,
				createDate.ToString(DATE_FORMAT),
				expirationDate.ToString(DATE_FORMAT),
				accessToken,
				refreshToken
			);
		}

		public TokenVO ValidateCredentials(RefreshTokenVO token) // trocado de TokenVO para RefreshTokenVO
		{
			var accessToken = token.AccessToken;
			var refreshToken = token.RefreshToken;

			var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

			var username = principal.Identity.Name;
			var user = _repository.ValidateCredentials(username);

			if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
				return null;

			accessToken = _tokenService.GenerateAccessToken(principal.Claims);
			refreshToken = _tokenService.GenerateRefreshToken();

			user.RefreshToken = refreshToken;
			_repository.RefreshUserInfo(user);

			DateTime createDate = DateTime.Now;
			DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

			return new TokenVO(
				true,
				createDate.ToString(DATE_FORMAT),
				expirationDate.ToString(DATE_FORMAT),
				accessToken,
				refreshToken
			);
		}

		public bool RevokeToken(string userName)
		{
			return _repository.RevokeToken(userName);
		}
	}
}
