using ApiAluguelCavalos.Domain.Dtos.Aluguel;
using ApiAluguelCavalos.Domain.Models;
using ApiAluguelCavalos.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiAluguelCavalos.Endpoints.Aluguel;

public class AluguelPut
{
    public static string Template => "/aluguel/{id}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action([FromRoute]Guid id, [FromBody] AluguelRequest inputAluguel, ApplicationDbContext db, HttpContext http)
    {
        var clienteId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var aluguel = await db.Alugueis.Include(a => a.Cliente).FirstAsync(a => a.Id.Equals(id));
        var cavalo = await db.Cavalos.FindAsync(inputAluguel.CavaloId);

        if (cavalo is CavaloModel Cavalo && aluguel is AluguelModel Aluguel)
        {
            aluguel.EditarInformacoes(inputAluguel.DataReserva, inputAluguel.NumeroHoras, cavalo);
            db.SaveChanges();

            var response = new AluguelResponse(aluguel.Id, aluguel.CavaloId, aluguel.ClienteId, aluguel.Cliente.Email, cavalo.Nome, aluguel.NumeroHoras, aluguel.DataReserva);

           return Results.Ok(response);
        }
 
        return Results.NotFound();
    }
}
