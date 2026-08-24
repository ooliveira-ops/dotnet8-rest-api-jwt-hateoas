using Microsoft.SqlServer.Management.Smo;
using RestWithASPNETUdemy.Data.VO;

namespace RestWithASPNETUdemy.Repository
{
	public interface IUserRepository
	{
		Model.User ValidateCredentials(UserVO user);
		Model.User ValidateCredentials(string username);
		bool RevokeToken(string username);

		Model.User RefreshUserInfo(Model.User user);
	}
}