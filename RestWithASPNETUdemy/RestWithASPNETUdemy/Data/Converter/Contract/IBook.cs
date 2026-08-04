namespace RestWithASPNETUdemy.Data.Converter.Contract
{
				//O = origem, D = destino
	public interface IBook<O, D>
	{
		//vai receber uma origem e transformar em um destino
		D Book(O origin);

		//vai receber uma lista de origem e transformar em uma lista de destino
		List<D> Book(List<O> origin);
	}
}
