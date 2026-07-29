using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Business;
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

		[HttpPost]
		public IActionResult Create([FromBody] Person person)
		{
			if (person == null)
				return BadRequest();

			return Ok(_personBusiness.Create(person));
		}

		[HttpPut]
		public IActionResult Update([FromBody] Person person)
		{
			if (person == null)
				return BadRequest();

			var updatedPerson = _personBusiness.Update(person);
			return Ok(updatedPerson);
		}

		[HttpGet("{id}")]
		public IActionResult FindById(long id)
		{
			var person = _personBusiness.FindById(id);
			if (person == null)
				return NotFound("Person not found");

			return Ok(person);
		}

		[HttpGet]
		public IActionResult FindAll()
		{
			var persons = _personBusiness.FindAll();
			return Ok(persons);
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