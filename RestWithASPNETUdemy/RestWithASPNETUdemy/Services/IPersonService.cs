using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Services
{
	public interface IPersonService
	{
		Person Create (Person person);
		Person FindById (long id);
		Person FindByName (string name);
		List<Person> FindAll ();
		Person Update (Person person);
		void Delete (long id);
	}
}
