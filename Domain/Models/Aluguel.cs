using ApiAluguelCavalos.Domain.Models.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;

namespace ApiAluguelCavalos.Domain.Models;

public class AluguelModel : EntidadeBase
{
	public Guid ClienteId { get; private set; }
	public Guid CavaloId { get; private set; }
	public DateTime DataReserva { get; private set; }
	public int NumeroHoras { get; private set; }
	public virtual CavaloModel Cavalo { get; set; }
	public virtual IdentityUser Cliente { get; set; }


	public AluguelModel(Guid clienteId, DateTime dataReserva, int numeroHoras, Guid cavaloId)
	{
		ClienteId = clienteId;
		CavaloId = cavaloId;
		DataReserva = dataReserva;
		NumeroHoras = numeroHoras;
	}

    public void EditarInformacoes(DateTime dataReserva, int numeroHoras, CavaloModel cavalo)
    {
        DataReserva = dataReserva;
        NumeroHoras = numeroHoras;
		CavaloId = cavalo.Id;
        Cavalo = cavalo;
    }


}
