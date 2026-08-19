using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using RestWithASPNETUdemy.Hypermedia.Abstract;

namespace RestWithASPNETUdemy.Hypermedia
{
	//classe abstrata que vai ser usada para enriquecer a resposta(HATEOAS)
	public abstract class ContentResponseEnricher<T> : IResponseEnricher where T : ISupportsHypermedia
	{
		public ContentResponseEnricher()
		{

		}

		//método que verifica se o tipo de conteúdo é do tipo T ou uma lista de T
		public virtual bool CanEnrich(Type contentType)
		{
			return contentType == typeof(T) || 
			contentType == typeof(List<T>);
		}

		//método abstrato que vai ser implementado nas classes filhas para enriquecer o modelo com links de hipermedia
		protected abstract Task EnrichModel(T content, IUrlHelper urlHelper);


		//método que verifica se o contexto da resposta pode ser HATEOAS(enriquecida com links de hipermedia)
		bool IResponseEnricher.CanEnrich(ResultExecutingContext context)
		{
			if (context.Result is ObjectResult objectResult)
			{
				return CanEnrich(objectResult.Value.GetType());
			}
			return false;
		}


		//método que enriquece a resposta da API com links de hipermedia
		//async para aguardar a conclusão do metodo sem bloquear a thread principal
		public async Task Enrich(ResultExecutingContext response)
		{
			var urlHelper = new UrlHelperFactory().GetUrlHelper(response);
			if (response.Result is ObjectResult okObjectResult)
			{
				if(okObjectResult.Value is T model)
				{
					await EnrichModel(model, urlHelper);				}
				else if (okObjectResult.Value is List<T> collection)
				{
					ConcurrentBag<T> bag = new ConcurrentBag<T>(collection);
					Parallel.ForEach(bag, (element) =>
					{
						EnrichModel(element, urlHelper).Wait();
					});
				}
				await Task.FromResult<object>(null);
			}
		}
	}
}
