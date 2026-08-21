using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Filters;
using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Controllers
{
	[ApiVersion("1")]
	[ApiController]
	[Authorize("Bearer")]
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

		[HttpGet]
		[ProducesResponseType((200), Type = typeof(List<BookVO>))]
		[ProducesResponseType((204))]
		[ProducesResponseType((400))]
		[ProducesResponseType((401))]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult FindAll()
		{
			var books = _bookBusiness.FindAll();
			return Ok(books);
		}

		[HttpGet("{id}")]
		[ProducesResponseType((200), Type = typeof(BookVO))]
		[ProducesResponseType((204))]
		[ProducesResponseType((400))]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult FindById(long id)
		{
			var book = _bookBusiness.FindById(id);
			if (book == null)
				return NotFound("Book not found");

			return Ok(book);
		}

		[HttpPost]
		[ProducesResponseType((200), Type = typeof(BookVO))]
		[ProducesResponseType((400))]
		[ProducesResponseType((401))]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Create([FromBody] BookVO book)
		{
			if (book == null)
				return BadRequest();

			return Ok(_bookBusiness.Create(book));
		}

		[HttpPut]
		[ProducesResponseType((200), Type = typeof(BookVO))]
		[ProducesResponseType((400))]
		[ProducesResponseType((401))]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Update([FromBody] BookVO book)
		{
			if (book == null)
				return BadRequest();

			var updatedBook = _bookBusiness.Update(book);
			return Ok(updatedBook);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType((204))]
		[ProducesResponseType((400))]
		[ProducesResponseType((401))]
		public IActionResult Delete(long id)
		{
			if (id <= 0)
				return BadRequest("Invalid Id");
			_bookBusiness.Delete(id);
			return NoContent();
		}
	}
}
