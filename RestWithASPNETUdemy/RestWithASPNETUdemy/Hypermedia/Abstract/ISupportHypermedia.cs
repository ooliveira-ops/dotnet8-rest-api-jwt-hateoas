namespace RestWithASPNETUdemy.Hypermedia.Abstract
{
	public interface ISupportHypermedia
	{ //aqui vai ser a implementação dos links de hypermedia
		List<HypermediaLink> Links { get; set; }
	}
}
