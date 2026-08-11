namespace RestWithASPNETUdemy.Hypermedia.Constants
{
	//  Os campos abaixo dizem qual o formato de retorno da API(no caso em JSON) 
	public sealed class ResponseTypeFormat
	{
		public const string DefaultGet = "application/json";
		public const string DefaultPost = "application/json";
		public const string DefaultPut = "application/json";
		public const string DefaultDelete = "application/json";
		public const string DefaultPatch = "application/json";
	}
}
