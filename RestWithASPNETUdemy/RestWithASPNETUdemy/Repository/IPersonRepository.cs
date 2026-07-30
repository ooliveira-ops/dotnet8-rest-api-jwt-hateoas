using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Repository
{
	public interface IRepository
	{
		Person Create (Person person);
		Person FindById (long id);
		List<Person> FindAll ();
		Person Update (Person person);
		void Delete (long id);

		bool Exists(long id);
	}
}
