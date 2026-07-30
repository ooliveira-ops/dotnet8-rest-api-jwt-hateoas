using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Controllers
{
	[ApiVersion("1")]
	[ApiController]
	[Route("api/[controller]/v{version:apiVersion}")]
	public class BookController : ControllerBase
	{

		private readonly ILogger<BookController> _logger;
		private readonly IBookBusiness _bookBusiness;

		//Injecao de dependencia
		public BookController(ILogger<BookController> logger, IBookBusiness bookBusiness)
		{   //construtor
			_logger = logger;
			_bookBusiness = bookBusiness;
		}

		[HttpPost]
		public IActionResult Create([FromBody] Book book)
		{
			if (book == null)
				return BadRequest();

			return Ok(_bookBusiness.Create(book));
		}

		[HttpPut]
		public IActionResult Update([FromBody] Book book)
		{
			if (book == null)
				return BadRequest();

			var updatedBook = _bookBusiness.Update(book);
			return Ok(updatedBook);
		}

		[HttpGet("{id}")]
		public IActionResult FindById(long id)
		{
			var book = _bookBusiness.FindById(id);
			if (book == null)
				return NotFound("Book not found");

			return Ok(book);
		}

		[HttpGet]
		public IActionResult FindAll()
		{
			var books = _bookBusiness.FindAll();
			return Ok(books);
		}


		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			if (id <= 0)
				return BadRequest("Invalid Id");
			_bookBusiness.Delete(id);
			return NoContent();
		}
	}
}
