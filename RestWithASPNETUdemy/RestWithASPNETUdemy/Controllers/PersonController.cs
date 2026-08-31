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
	public class PersonController : ControllerBase
	{

		private readonly ILogger<PersonController> _logger;
		private readonly IPersonBusiness _personBusiness;

		//Injecao de dependencia
		public PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness)
		{	//construtor
			_logger = logger;
			_personBusiness = personBusiness;			
		}

		[HttpGet("{sortDirection}/{pageSize}/{page}")]
		[ProducesResponseType((200), Type = typeof(List<PersonVO>))]
		[ProducesResponseType((204))]
		[ProducesResponseType((400))]
		[ProducesResponseType((401))]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Get(string sortDirection, int pageSize, int page, [FromQuery] string? name = null)
		{
			return Ok(_personBusiness.FindWithPagedSearch(name, sortDirection, pageSize, page));
		}

		[HttpGet("{id}")]
		[ProducesResponseType((200), Type = typeof(PersonVO))]
		[ProducesResponseType((204))]
		[ProducesResponseType((400))]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult FindById(long id)
		{
			var person = _personBusiness.FindById(id);
			if (person == null)
				return NotFound("PersonVO not found");

			return Ok(person);
		}

		[HttpGet("findPersonByName")]
		[ProducesResponseType((200), Type = typeof(PersonVO))]
		[ProducesResponseType((204))]
		[ProducesResponseType((400))]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Get([FromQuery] string? firstName = null, [FromQuery] string? lastName = null)
		{
			var person = _personBusiness.FindByName(firstName, lastName);
			if (person == null)
				return NotFound("PersonVO not found");

			return Ok(person);
		}

		[HttpPost]
		[ProducesResponseType((200), Type = typeof(PersonVO))]
		[ProducesResponseType((400))]
		[ProducesResponseType((401))]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Create([FromBody] PersonVO person)
		{
			if (person == null)
				return BadRequest();

			return Ok(_personBusiness.Create(person));
		}

		[HttpPut]
		[ProducesResponseType((200), Type = typeof(PersonVO))]
		[ProducesResponseType((400))]
		[ProducesResponseType((401))]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Update([FromBody] PersonVO person)
		{
			if (person == null)
				return BadRequest();

			var updatedPerson = _personBusiness.Update(person);
			return Ok(updatedPerson);
		}


		[HttpPatch("{id}")]
		[ProducesResponseType((200), Type = typeof(List<PersonVO>))]
		[ProducesResponseType((204))]
		[ProducesResponseType((400))]
		[ProducesResponseType((401))]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Patch(long id)
		{
			var persons = _personBusiness.Disable(id);
			return Ok(persons);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType((204))]
		[ProducesResponseType((400))]
		[ProducesResponseType((401))]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Delete(long id)
		{
			if (id <= 0)
			return BadRequest("Invalid Id");
			_personBusiness.Delete(id);
			return NoContent();
		}
	}
}