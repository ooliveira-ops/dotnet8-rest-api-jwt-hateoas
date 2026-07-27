using Microsoft.AspNetCore.Mvc;

namespace RestWithASPNETUdemy.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class CalculatorController : ControllerBase
	{

		private readonly ILogger<CalculatorController> _logger;
		public CalculatorController(ILogger<CalculatorController> logger)
		{
			_logger = logger;
		}

		[HttpGet("sum/{firstNumber}/{secondNumber}")]
		public IActionResult Sum(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				var sum = ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber);

				return Ok(sum.ToString());
			}
			return BadRequest("Invalid Input");
		}

		[HttpGet("sub/{firstNumber}/{secondNumber}")]
		public IActionResult Sub(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				var sub = ConvertToDecimal(firstNumber) - ConvertToDecimal(secondNumber);

				return Ok(sub.ToString());
			}
			return BadRequest("Invalid Input");
		}


		[HttpGet("mult/{firstNumber}/{secondNumber}")]
		public IActionResult Mult(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				var sub = ConvertToDecimal(firstNumber) * ConvertToDecimal(secondNumber);

				return Ok(sub.ToString());
			}
			return BadRequest("Invalid Input");
		}


		[HttpGet("med/{firstNumber}/{secondNumber}")]
		public IActionResult Div(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				var sub = (ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber)) / 2;

				return Ok(sub.ToString());
			}
			return BadRequest("Invalid Input");

		}


		[HttpGet("sqroot/{firstNumber}")]
		public IActionResult SquareRoot(string firstNumber)
		{
			if (IsNumeric(firstNumber))
			{
				var square = Math.Sqrt((double)ConvertToDecimal(firstNumber));

				return Ok(square.ToString());
			}
			return BadRequest("Invalid Input");

		}


		private bool IsNumeric(string strNumber)
		{
			double number;
			bool isNumber = double.TryParse(
			strNumber,
			System.Globalization.NumberStyles.Any,
			System.Globalization.NumberFormatInfo.InvariantInfo,
			out number);
			return isNumber;
		}

		private decimal ConvertToDecimal(string strNumber)
		{
			decimal decimalValue;
			if (decimal.TryParse(strNumber, out decimalValue))
			{
				return (decimalValue);
			}
			return 0;
		}

	}
}