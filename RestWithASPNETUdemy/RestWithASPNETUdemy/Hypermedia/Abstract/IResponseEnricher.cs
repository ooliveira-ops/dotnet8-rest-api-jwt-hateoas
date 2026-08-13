using Microsoft.AspNetCore.Mvc.Filters;

namespace RestWithASPNETUdemy.Hypermedia.Abstract
{   
	public interface IResponseEnricher
	{   //esse CanEnrich verifica se a resposta pode ser enriquecida
		bool CanEnrich(ResultExecutingContext context);

		//esse Enrich enriquece a resposta
		Task Enrich(ResultExecutingContext context);
	}
}
