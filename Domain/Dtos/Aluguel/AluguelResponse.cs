namespace ApiAluguelCavalos.Domain.Dtos.Aluguel;

public record AluguelResponse(Guid Id, Guid CavaloId, Guid UsuarioId, string UsuarioEmail, string CavaloNome, int NumeroHoras, DateTime DataReserva);
