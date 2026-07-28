using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Services;
using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class PersonController : ControllerBase
	{

		private readonly ILogger<PersonController> _logger;
		private readonly IPersonService _personService;

		//Injecao de dependencia
		public PersonController(ILogger<PersonController> logger, IPersonService personService)
		{	//construtor
			_logger = logger;
			_personService = personService;			
		}

		[HttpPost]
		public IActionResult Create([FromBody] Person person)
		{
			if (person == null)
				return BadRequest();

			return Ok(_personService.Create(person));
		}

		[HttpPut]
		public IActionResult Update([FromBody] Person person)
		{
			if (person == null)
				return BadRequest();

			var updatedPerson = _personService.Update(person);
			return Ok(updatedPerson);
		}

		[HttpGet("{id}")]
		public IActionResult FindById(long id)
		{
			var person = _personService.FindById(id);
			if (person == null)
				return NotFound("Person not found");

			return Ok(person);
		}

		[HttpGet]
		public IActionResult FindAll()
		{
			var persons = _personService.FindAll();
			return Ok(persons);
		}


		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			if (id <= 0)
			return BadRequest("Invalid Id");
			_personService.Delete(id);
			return NoContent();
		}
	}
}