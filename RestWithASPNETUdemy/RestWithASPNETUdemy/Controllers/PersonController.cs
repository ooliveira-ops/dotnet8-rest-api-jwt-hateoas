using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Filters;
using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Controllers
{

	[ApiVersion("1")]
	[ApiController]
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

		[HttpGet]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult FindAll()
		{
			var persons = _personBusiness.FindAll();
			return Ok(persons);
		}

		[HttpGet("{id}")]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult FindById(long id)
		{
			var person = _personBusiness.FindById(id);
			if (person == null)
				return NotFound("PersonVO not found");

			return Ok(person);
		}

		[HttpPost]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Create([FromBody] PersonVO person)
		{
			if (person == null)
				return BadRequest();

			return Ok(_personBusiness.Create(person));
		}

		[HttpPut]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Update([FromBody] PersonVO person)
		{
			if (person == null)
				return BadRequest();

			var updatedPerson = _personBusiness.Update(person);
			return Ok(updatedPerson);
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			if (id <= 0)
			return BadRequest("Invalid Id");
			_personBusiness.Delete(id);
			return NoContent();
		}
	}
}