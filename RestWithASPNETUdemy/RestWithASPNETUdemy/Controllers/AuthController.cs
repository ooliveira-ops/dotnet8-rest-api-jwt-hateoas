using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Data.VO;

namespace RestWithASPNETUdemy.Controllers
{
	[ApiVersion("1")]
	[Route("api/[controller]/v{version:apiVersion}")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private ILoginBusiness _loginBusiness;

		public AuthController(ILoginBusiness loginBusiness)
		{
			_loginBusiness = loginBusiness;
		}

		// Metodo que realiza o login do usuário com a rota definida e o "FromBody"
		// para receber o objeto UserVO no corpo da requisição
		[HttpPost]
		[Route("signin")]
		public IActionResult Signin([FromBody] UserVO user)
		{
			if (user == null) return BadRequest("Invalid client request");
			var token = _loginBusiness.ValidateCredentials(user);
			if (token == null) return Unauthorized();
			return Ok(token);
		}

		// Metodo que realiza o refresh do token do usuário com a rota definida e o "FromBody"
		// para receber o objeto TokenVO no corpo da requisição
		[HttpPost]
		[Route("refresh")]
		public IActionResult Refresh([FromBody] RefreshTokenVO tokenVo)
		{
			if (tokenVo == null) return BadRequest("Invalid client request");
			var token = _loginBusiness.ValidateCredentials(tokenVo);
			if (token == null) return BadRequest("Invalid client request");
			return Ok(token);
		}

		// Metodo que revoga o token do usuário com a rota definida e o
		// "Authorize" para exigir autenticação
		[HttpGet]
		[Route("revoke")]
		[Authorize("Bearer")]
		public IActionResult Revoke()
		{
			var username = User.Identity.Name;
			var result = _loginBusiness.RevokeToken(username);
			if (!result) return BadRequest("Invalid client request");
			return NoContent();
		}
	}
}
