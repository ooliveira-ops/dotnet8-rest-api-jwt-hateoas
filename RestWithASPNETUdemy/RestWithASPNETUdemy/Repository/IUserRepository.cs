using Microsoft.SqlServer.Management.Smo;
using RestWithASPNETUdemy.Data.VO;

namespace RestWithASPNETUdemy.Repository
{
	public interface IUserRepository
	{
		public Model.User ValidateCredentials(UserVO user);
	}
}
