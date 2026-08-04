using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Data.Converter.Contract.Implementations
{                        //Converter: BookVO -> Book || Book -> BookVO	
	public class BookConverter : IBook<BookVO, Book>, IBook<Book, BookVO>
	{
		public Book Book(BookVO origin)
		{ //Ele verifica se o objeto de origem é nulo e, em caso afirmativo, retorna nulo.
		//Caso contrário, ele cria um novo objeto Book e copia os valores das propriedades do objeto de origem para o novo objeto.

			if (origin == null) return null;
			return new Book
			{
				Id = origin.Id,
				Title = origin.Title,
				Author = origin.Author,
				Price = origin.Price,
				LaunchDate = origin.LaunchDate
			};
		}


		public BookVO Book(Book origin)
		{
			if (origin == null) return null;
			return new BookVO
			{
				Id = origin.Id,
				Title = origin.Title,
				Author = origin.Author,
				Price = origin.Price,
				LaunchDate = origin.LaunchDate
			};
		}

		//Aqui temos dois métodos Book que recebem uma lista de objetos de origem (BookVO ou Book).
		//Eles utilizam o método Select do LINQ para aplicar a conversão a cada item da lista de origem e, em seguida, convertem o resultado em uma lista usando ToList().
		public List<Book> Book(List<BookVO> origin)
		{
			if (origin == null) return null;
			return origin.Select(item => Book(item)).ToList(); //lista com os objetos convertidos
		}


		//Aqui temos dois métodos Book que recebem uma lista de objetos de origem (BookVO ou Book).
		public List<BookVO> Book(List<Book> origin)
		{
			if (origin == null) return null;
			return origin.Select(item => Book(item)).ToList(); //lista com os objetos convertidos
		}
	}
}
