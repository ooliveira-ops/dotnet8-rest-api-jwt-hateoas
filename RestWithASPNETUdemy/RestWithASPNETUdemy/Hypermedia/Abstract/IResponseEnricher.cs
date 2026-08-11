using Microsoft.AspNetCore.Mvc.Filters;

namespace RestWithASPNETUdemy.Hypermedia.Abstract
{   
	public interface IResponseEnricher
	{   //esse CanEnrich é para verificar se o tipo de objeto que está sendo retornado é do tipo que o Enricher consegue enriquecer, ou seja, se ele consegue adicionar os links de hypermedia
		bool CanEnrich(ResultExecutedContext context);

		//esse Enrich é para adicionar os links de hypermedia no objeto que está sendo retornado
		Task Enrich(ResultExecutedContext context);
	}
}
