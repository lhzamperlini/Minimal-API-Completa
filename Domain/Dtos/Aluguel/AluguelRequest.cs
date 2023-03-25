namespace ApiAluguelCavalos.Domain.Dtos.Aluguel;

public record AluguelRequest(Guid CavaloId, DateTime DataReserva, int NumeroHoras);
