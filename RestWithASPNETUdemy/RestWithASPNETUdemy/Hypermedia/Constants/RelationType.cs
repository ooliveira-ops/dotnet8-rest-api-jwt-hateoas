namespace RestWithASPNETUdemy.Hypermedia.Constants
{
	//  resumo: aqui define os tipos de relação que vão ser usados na aplicação(para definir as ações que vão ser feitas nos recursos da API)
	public sealed class RelationType
	{
		public const string self = "self";
		public const string post = "post";
		public const string put = "put";
		public const string delete = "delete";
		public const string patch = "patch";
		public const string next = "next";
		public const string previous = "previous";
		public const string first = "first";
		public const string last = "last";
	}
}
