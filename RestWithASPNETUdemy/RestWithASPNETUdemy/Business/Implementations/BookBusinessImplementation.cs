using RestWithASPNETUdemy.Data.Converter.Contract.Implementations;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Repository;

namespace RestWithASPNETUdemy.Business.Implementations
{
	public class BookBusinessImplementation : IBookBusiness
	{
		private readonly IRepository<Book> _repository;
		private readonly BookConverter _converter;

		public BookBusinessImplementation(IRepository<Book> repository)
		{
			_repository = repository;
			_converter = new BookConverter();
		}

		public List<BookVO> FindAll()
		{
			return _converter.Book(_repository.FindAll());
		}

		public BookVO FindById(long id)
		{
			return _converter.Book(_repository.FindById(id));
		}

		//O objeto chega como VO, é convertido para Entity, e depois de persistido, é convertido novamente para VO
		public BookVO Create(BookVO book)
		{
			var bookEntity = _converter.Book(book);
			bookEntity = _repository.Create(bookEntity);
			return _converter.Book(bookEntity);
		}

		public BookVO Update(BookVO book)
		{
			var bookEntity = _converter.Book(book);
			bookEntity = _repository.Update(bookEntity);
			return _converter.Book(bookEntity);
		}

		public void Delete(long id)
		{
			_repository.Delete(id);
		}
	}
}