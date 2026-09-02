using RestWithASPNETUdemy.Data.VO;

namespace RestWithASPNETUdemy.Business.Implementations
{
	public class FileBusinessImplementation : IFileBusiness
	{

		private readonly string _basePath;
		private readonly IHttpContextAccessor _context;

		public FileBusinessImplementation(IHttpContextAccessor context)
		{
			_context = context;
			_basePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
		}


		public byte[] GetFile(string fileName)
		{
			var filepath = _basePath + fileName;
			return File.ReadAllBytes(filepath);
		}


		// Aqui é a lógica para salvar o arquivo no disco
		public async Task<FileDetailVO> SaveFileToDisk(IFormFile file)
		{
			FileDetailVO FileDetail = new FileDetailVO();

			// Pega o tipo do arquivo
			var fileType = Path.GetExtension(file.FileName);
			// Pega o nome do host
			var baseUrl = _context.HttpContext.Request.Host;

			// Verifica se o tipo do arquivo é permitido
			if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" ||
				fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
			{
				var docName = Path.GetFileName(file.FileName);
				if (file != null && file.Length > 0)
				{
					var destination = Path.Combine(_basePath, "", docName);
					FileDetail.DocumentName = docName;
					FileDetail.DocType = fileType;
					FileDetail.DocUrl = Path.Combine(baseUrl + "/api/file/v1/" + FileDetail.DocumentName);

					// Cria uma pasta para armazenar o arquivo
					using var stream = new FileStream(destination, FileMode.Create);
					// Copia o arquivo para o disco
					await file.CopyToAsync(stream);
				}
				else
				{
					throw new Exception("File is empty.");
				}
			}
			else
			{
				throw new Exception("Invalid file type. Only PDF, JPG, PNG and JPEG are allowed.");
			}
			return FileDetail;
		}

		// Aqui é a lógica para salvar vários arquivos no disco
		public async Task<List<FileDetailVO>> SaveFilesToDisk(IList<IFormFile> files)
		{
			List<FileDetailVO> list = new List<FileDetailVO>();
			foreach (var file in files)
			{
				list.Add(await SaveFileToDisk(file));
			}
			return list;
		}

	}
}
