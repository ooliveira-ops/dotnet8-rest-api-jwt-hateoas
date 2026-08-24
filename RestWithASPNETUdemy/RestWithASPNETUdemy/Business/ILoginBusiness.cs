using RestWithASPNETUdemy.Data.VO;

namespace RestWithASPNETUdemy.Business
{
	public interface ILoginBusiness
	{
		TokenVO ValidateCredentials(UserVO user);
		TokenVO ValidateCredentials(RefreshTokenVO token);
		bool RevokeToken(string userName);
	}
}
