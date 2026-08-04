namespace RestWithASPNETUdemy.Data.Converter.Contract
{
				//O = origem, D = destino
	public interface IParser<O, D>  
	{	
		//vai receber uma origem e transformar em um destino
		D Parse(O origin);

		//vai receber uma lista de origem e transformar em uma lista de destino
		List<D> Parse(List<O> origin);
	}
}
