namespace RestWithASPNETUdemy.Hypermedia.Abstract
{
	public interface ISupportsHypermedia
	{ //aqui vai ser a implementação dos links de hypermedia
		List<HyperMediaLink> Links { get; set; }
	}
}
