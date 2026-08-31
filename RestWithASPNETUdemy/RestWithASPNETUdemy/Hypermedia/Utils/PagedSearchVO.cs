using RestWithASPNETUdemy.Hypermedia.Abstract;

namespace RestWithASPNETUdemy.Hypermedia.Utils
{
	// Essa classe é utilizada para representar uma pesquisa paginada,
	// contendo informações sobre a página atual, tamanho da página, total de resultados,
	// campos de ordenação, direções de ordenação, filtros aplicados e a lista de resultados.
	public class PagedSearchVO<T> where T : ISupportsHypermedia
	{
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public int TotalResults { get; set; }
		public string SortFields { get; set; }
		public string SortDirections { get; set; }
		public Dictionary<string, Object> Filters { get; set; }
		public List<T> List { get; set; }


		public PagedSearchVO() {}


		public PagedSearchVO(int currentPage, int pageSize, string sortFields, string sortDirections)
		{
			CurrentPage = currentPage;
			PageSize = pageSize;
			SortFields = sortFields;
			SortDirections = sortDirections;
		}


		public PagedSearchVO(int currentPage, int pageSize, string sortFields, string sortDirections, Dictionary<string, object> filters)
		{
			CurrentPage = currentPage;
			PageSize = pageSize;
			SortFields = sortFields;
			SortDirections = sortDirections;
			Filters = filters;
		}


		public PagedSearchVO(int currentPage, string sortFields, string sortDirections)
			: this(currentPage, 10, sortFields, sortDirections) {}


		public int GetCurrentPage()
		{
			 return CurrentPage == 0 ? 2 : CurrentPage;
		}

		public int GetPageSize()
		{
			 return PageSize == 0 ? 10 : PageSize;
		}
	}
}
