using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Data.VO;
namespace RestWithASPNETUdemy.Controllers
{
	[ApiVersion("1")]
	[ApiController]
	[Authorize("Bearer")]
	[Route("api/[controller]/v{version:apiVersion}")]
	public class FileController : ControllerBase
	{
		private readonly IFileBusiness _fileBusiness;
		public FileController(IFileBusiness fileBusiness)
		{
			_fileBusiness = fileBusiness;
		}

		[HttpPost("uploadFile")]
		[ProducesResponseType((200), Type = typeof(FileDetailVO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public async Task<IActionResult> UploadOneFile(IFormFile file)
		{
			FileDetailVO detail = await _fileBusiness.SaveFileToDisk(file);
			return new OkObjectResult(detail);
		}


		[HttpGet("downloadFile/{fileName}")]
		[ProducesResponseType((200), Type = typeof(byte[]))]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		[Produces("application/octet-stream")]
		public async Task<IActionResult> GetFileAsync(string fileName)
		{
			byte[] buffer = _fileBusiness.GetFile(fileName);
			if (buffer == null) return NotFound();

			string contentType = "application/octet-stream";
			var provider = new FileExtensionContentTypeProvider();
			if (provider.TryGetContentType(fileName, out var detectedType))
				contentType = detectedType;

			return File(buffer, contentType, fileName);
		}
	

		[HttpPost("uploadMultipleFile")]
		[ProducesResponseType((200), Type = typeof(List<FileDetailVO>))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		public async Task<IActionResult> UploadManyFiles(List<IFormFile> files)
		{
			List<FileDetailVO> details = await _fileBusiness.SaveFilesToDisk(files);
			return new OkObjectResult(details);
		}
	}
}
