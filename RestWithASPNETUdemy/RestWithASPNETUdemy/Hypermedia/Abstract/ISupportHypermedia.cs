namespace RestWithASPNETUdemy.Hypermedia.Abstract
{
	public interface ISupportHypermedia
	{ //aqui vai ser a implementação dos links de hypermedia
		List<HyperMediaLink> Links { get; set; }
	}
}
