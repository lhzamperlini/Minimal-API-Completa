namespace ApiAluguelCavalos.Domain.Models.Base;

public abstract class EntidadeBase
{
	public EntidadeBase()
	{
		Id = Guid.NewGuid();
		CriadoEm = DateTime.Now;
		EditadoEm = DateTime.Now;
	}

	public Guid Id { get; set; }
	public DateTime CriadoEm { get; set; }
	public DateTime EditadoEm { get; set; }

}
