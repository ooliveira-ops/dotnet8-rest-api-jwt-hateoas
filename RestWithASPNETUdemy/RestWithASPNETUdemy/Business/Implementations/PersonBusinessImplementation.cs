using System.Security.Cryptography;
using Microsoft.SqlServer.Server;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Data.Converter.Contract.Implementations;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Utils;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository;

namespace RestWithASPNETUdemy.Business.Implementations
{
	public class PersonBusinessImplementation : IPersonBusiness
	{

		private readonly IPersonRepository _repository;

		private readonly PersonConverter _converter;

		public PersonBusinessImplementation(IPersonRepository repository)
		{
			_repository = repository;
			_converter = new PersonConverter();
		}

		public List<PersonVO> FindAll()
		{
			return _converter.Parse(_repository.FindAll());
		}

		// Aqui é a lógica de pesquisa paginada.
		// O método FindWithPagedSearch recebe parâmetros para nome, direção de ordenação, tamanho da página e número da página.
		// Ele calcula o deslocamento (offset) com base no número da página e no tamanho da página, define a direção de ordenação
		// e cria a consulta SQL para buscar os registros correspondentes. Em seguida, ele chama o repositório para executar a consulta
		// e obter os resultados paginados, bem como o total de resultados. Por fim, ele retorna um objeto PagedSearchVO contendo as informações da pesquisa paginada.
		public PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
		{
			var sort = (!string.IsNullOrWhiteSpace(sortDirection) && sortDirection.Equals("desc")) ? "desc" : "asc";
			var size = (pageSize < 1) ? 10 : pageSize;
			var offset = page > 0 ? (page - 1) * size : 0;

			// Monta a consulta SQL para BUSCAR OS REGISTROS de pessoas com base nos parâmetros fornecidos
			string query = @"select * from person p where 1 = 1 ";
			if (!string.IsNullOrWhiteSpace(name)) query = query + $" and p.first_name like '%{name}%' ";
			query += $" order by p.first_name {sort} offset {offset} rows fetch next {size} rows only";

			// Monta a consulta SQL para contar o TOTAL DE REGISTROS correspondentes à pesquisa
			string countQuery = @"select count(*) from person p where 1 = 1 ";
			if (!string.IsNullOrWhiteSpace(name)) countQuery = countQuery + $" and p.first_name like '%{name}%' ";

			// Executa a consulta para buscar os registros de pessoas e o total de resultados
			var persons = _repository.FindWithPagedSearch(query);
			int totalResults = _repository.GetCount(countQuery);

			// Retorna um objeto PagedSearchVO contendo as informações da pesquisa paginada
			return new PagedSearchVO<PersonVO>
			{
				CurrentPage = page,
				List = _converter.Parse(persons),
				PageSize = size,
				SortDirections = sort,
				TotalResults = totalResults,
			};
		}


		public PersonVO FindById(long id)
		{
			return _converter.Parse(_repository.FindById(id));
		}

		public List<PersonVO> FindByName(string firstName, string lastName)
		{
			return _converter.Parse(_repository.FindByName(firstName, lastName));
		}

		//O objeto chega como VO, é convertido para Entity, e depois de persistido, é convertido novamente para VO
		public PersonVO Create(PersonVO person)
		{
			var personEntity = _converter.Parse(person);
			personEntity = _repository.Create(personEntity);
			return _converter.Parse(personEntity);
		}

		public PersonVO Update(PersonVO person)
		{
			var personEntity = _converter.Parse(person);
			personEntity = _repository.Update(personEntity);
			return _converter.Parse(personEntity);
		}


		public PersonVO Disable(long id)
		{
			var personEntity = _repository.Disable(id);
			return _converter.Parse(personEntity);
		}

		public void Delete(long id)
		{
			_repository.Delete(id);
		}

	}
}
