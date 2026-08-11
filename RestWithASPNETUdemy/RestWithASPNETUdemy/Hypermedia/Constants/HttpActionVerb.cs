namespace RestWithASPNETUdemy.Hypermedia.Constants
{
	// aqui define os verbos HTTP que vão ser usados na aplicação(vai usar os verbos HTTP para definir as ações que vão ser feitas nos recursos da API)
	public sealed class HttpActionVerb
	{
		public const string GET = "GET";
		public const string POST = "POST";
		public const string PUT = "PUT";
		public const string DELETE = "DELETE";
		public const string PATCH = "PATCH";
	}
}
