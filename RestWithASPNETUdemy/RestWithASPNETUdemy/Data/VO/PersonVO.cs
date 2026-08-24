using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestWithASPNETUdemy.Hypermedia;
using RestWithASPNETUdemy.Hypermedia.Abstract;
using RestWithASPNETUdemy.Model.Base;

namespace RestWithASPNETUdemy.Data.VO
{
	public class PersonVO : ISupportsHypermedia
	{
		// Custom Serialization é feito com o atributo JsonPropertyName, que permite definir o nome da propriedade no JSON
		// [JsonPropertyName("code")]
		public long Id { get; set; }

		// [JsonPropertyName("name")]
		public string FirstName { get; set; }

		// [JsonPropertyName("last_name")]
		public string LastName { get; set; }

		// [JsonIgnore] -> ignora o campo na serialização
		public string Address { get; set;}

		// [JsonPropertyName("sex")]
		public string Gender { get; set;}

		public bool Enabled { get; set; }

		// Suporta Hypermedia Links
		public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
	}
}
