using RestWithASPNETUdemy.Hypermedia.Abstract;

namespace RestWithASPNETUdemy.Hypermedia.Filters
{
	public class HyperMediaFilterOptions
	{   // essa lista vai ser usada para armazenar os enriquecedores de resposta que vão ser usados na aplicação
		public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
	}
}
