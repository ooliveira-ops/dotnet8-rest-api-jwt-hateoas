using System.Data;
using System.Security.Cryptography;
using System.Text;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;

namespace RestWithASPNETUdemy.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly SQLServerContext _context;

		// Construtor para injetar o contexto do banco de dados no repositório
		public UserRepository(SQLServerContext context)
		{
			_context = context;
		}

		// Método para validar as credenciais do usuário
		public Model.User ValidateCredentials(UserVO user)
		{
			var pass = ComputeHash(user.Password, SHA256.Create());
			return _context.Users.SingleOrDefault(u => (u.UserName == user.UserName) && (u.Password == pass));
		}

		// Método para validar as credenciais do usuário pelo nome de usuário
		public User ValidateCredentials(string userName)
		{
			return _context.Users.SingleOrDefault(u => (u.UserName == userName));
		}

		// Método para revogar o token de atualização do usuário
		public bool RevokeToken(string userName)
		{
			// Começa verificando se o usuário existe no banco de dados
			var user = _context.Users.SingleOrDefault(u => (u.UserName == userName));
			if (user is null) return false;
			user.RefreshToken = null;
			_context.SaveChanges();
			return true;
		}

		// Método para atualizar as informações do usuário
		public Model.User RefreshUserInfo(Model.User user)
		{
			// Verifica se: NÃO encontrar ninguem no banco com o mesmo ID e com o ID do user recebido = retorna nulo
			if (!_context.Users.Any(u => u.Id.Equals(user.Id))) return null;

			// Se encontrar alguem no banco com o mesmo ID e com o ID do user recebido = atualiza as informações do user
			var result = _context.Users.SingleOrDefault(p => p.Id.Equals(user.Id));
			if (result != null)
			{
				try
				{
					_context.Entry(result).CurrentValues.SetValues(user);
					_context.SaveChanges();
					return result;
				}
				catch (Exception)
				{
					throw;
				}
			}
			return result;
		}

		// Método para gerar o hash da senha
		private string ComputeHash(string input, SHA256 algorithm)
		{
			Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
			Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
			return BitConverter.ToString(hashedBytes);
		}

	}
}
