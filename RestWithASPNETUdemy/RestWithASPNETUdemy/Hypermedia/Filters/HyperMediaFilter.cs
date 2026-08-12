using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestWithASPNETUdemy.Hypermedia.Filters
{
	public class HyperMediaFilter  : ResultFilterAttribute
	{
		private readonly HyperMediaFilterOptions _hyperMediaFilterOptions;

		public HyperMediaFilter(HyperMediaFilterOptions hyperMediaFilterOptions)
		{
			_hyperMediaFilterOptions = hyperMediaFilterOptions;
		}

		public override void OnResultExecuting(ResultExecutingContext context)
		{
			// vai tentar adicionar os links de hypermedia no objeto que está sendo retornado, caso o
			// tipo do objeto seja do tipo que o Enricher consegue enriquecer
			TryEnrichResult(context);
			base.OnResultExecuting(context);
		}

		private void TryEnrichResult(ResultExecutingContext context)
		{
			// Verifica se o resultado da chamada foi um ObjectResult
			if (context.Result is ObjectResult objectResult)
			{
				// Verifica se o ObjectResult possui um valor
				var enricher = _hyperMediaFilterOptions.ContentResponseEnricherList
					.FirstOrDefault(x => x.CanEnrich(context));
				if (enricher != null)
				{
					// Se encontrou um Enricher que pode enriquecer o resultado, chama o método Enrich
					// usamos "Task.FromResult" para aguardar a conclusão
					Task.FromResult(enricher.Enrich(context));
				}
			}
			else
			{
				// Se o resultado não for um ObjectResult ou se o valor for nulo, não faz nada
			}
		}
	}
}
