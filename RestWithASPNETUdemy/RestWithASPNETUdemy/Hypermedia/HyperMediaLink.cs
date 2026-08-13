using System.Text;

namespace RestWithASPNETUdemy.Hypermedia
{
	public class HyperMediaLink
	{
		//relacionamento do link com o recurso
		public string Rel { get; set; }

		//tipo de link que vai ser usado
		private string href;

		//tipo de link que vai ser usado 
		public string Href 
		{
			get
			{
				object _lock = new object();
				lock (_lock)
				{
					//aqui vai ser feito o replace do "%2F" para '/' para que o link seja exibido corretamente
					StringBuilder sb = new StringBuilder(href);
					return sb.Replace("%2F", "/").ToString();
				}
			}
			set
			{	
				href = value;
			}
		}

		//tipo de link que vai ser usado
		public string Type { get; set; }

		//tipo de ação que vai ser feita no link
		public string Action { get; set; }
	}
}
