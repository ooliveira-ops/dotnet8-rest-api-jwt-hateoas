using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository.Generic;

namespace RestWithASPNETUdemy.Repository
{
	// Essa classe herda da classe genérica GenericRepository e implementa a interface IPersonRepository,
	// fornecendo métodos específicos para manipulação de entidades do tipo Person no banco de dados.
	public class PersonRepository : GenericRepository<Person>, IPersonRepository
	{
		public PersonRepository(SQLServerContext context) : base(context)
		{

		}

		// O método Disable é responsável por desabilitar uma pessoa no banco de dados,
		// alterando o valor da propriedade Enabled para false.
		public Person Disable(long id)
		{
			if (!_context.Persons.Any(p => p.Id.Equals(id))) return null;
			var user = _context.Persons.SingleOrDefault(p => p.Id.Equals(id));
			if (user != null)
			{
				user.Enabled = false;
				try
				{
					_context.Entry(user).CurrentValues.SetValues(user);
					_context.SaveChanges();
					return user;
				}
				catch (Exception)
				{
					throw;
				}	
			}
			else
			{
				return null;
			}
		}

		// O método FindByName é responsável por buscar pessoas no banco de dados
		// com base no primeiro nome e sobrenome fornecidos.
		public List<Person> FindByName(string firstName, string lastName)
		{
			// As 3 condições abaixo resultam na mesma consulta ao banco de dados.
			if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
			{
				return _context.Persons.Where(
				p => p.FirstName.Contains(firstName) 
				&& p.LastName.Contains(lastName)).ToList();
			}
			else if (string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
			{
				return _context.Persons.Where(
				 p => p.LastName.Contains(lastName)).ToList();
			}
			else if (!string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
			{
				return _context.Persons.Where(
				p => p.FirstName.Contains(firstName)).ToList();
			}
			return null;
		}
	}
}
