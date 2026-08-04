using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Data.Converter.Contract.Implementations
{							    //Converter: PersonVO -> Person || Person -> PersonVO
	public class PersonConverter : IParser<PersonVO, Person>, IParser<Person, PersonVO>
	{
		public Person Parse(PersonVO origin)
		{ //Ele verifica se o objeto de origem é nulo e, em caso afirmativo, retorna nulo.
		  //Caso contrário, ele cria um novo objeto Person e copia os valores das propriedades do objeto de origem para o novo objeto.

			if (origin == null) return null;
			return new Person
			{
				Id = origin.Id,
				FirstName = origin.FirstName,
				LastName = origin.LastName,
				Address = origin.Address,
				Gender = origin.Gender
			};
		}

		
		public PersonVO Parse(Person origin)
		{
			if (origin == null) return null;
			return new PersonVO
			{
				Id = origin.Id,
				FirstName = origin.FirstName,
				LastName = origin.LastName,
				Address = origin.Address,
				Gender = origin.Gender
			};
		}

		//Aqui temos dois métodos Parse que recebem uma lista de objetos de origem (PersonVO ou Person).
		//Eles utilizam o método Select do LINQ para aplicar a conversão a cada item da lista de origem e, em seguida, convertem o resultado em uma lista usando ToList().
		public List<Person> Parse(List<PersonVO> origin)
		{
			if (origin == null) return null;
			return origin.Select(item => Parse(item)).ToList(); //lista com os objetos convertidos
		}


		//Aqui temos dois métodos Parse que recebem uma lista de objetos de origem (PersonVO ou Person).
		public List<PersonVO> Parse(List<Person> origin)
		{
			if (origin == null) return null;
			return origin.Select(item => Parse(item)).ToList(); //lista com os objetos convertidos
		}
	}
}
