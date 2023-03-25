using ApiAluguelCavalos.Domain.Dtos.Aluguel;
using ApiAluguelCavalos.Domain.Models;
using ApiAluguelCavalos.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiAluguelCavalos.Endpoints.Aluguel;

public class AluguelGet
{
    public static string Template => "/aluguel";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(ApplicationDbContext db, UserManager<IdentityUser> userManager, HttpContext http)
    {
        var clienteId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var alugueis = await db.Alugueis.Include(a => a.Cavalo).Include(a => a.Cliente)
                       .Where(a => a.Cliente.Id.Equals(clienteId))
                       .Select(a => new AluguelResponse(a.Id, a.CavaloId, a.ClienteId, a.Cliente.Email, a.Cavalo.Nome, a.NumeroHoras, a.DataReserva))
                       .ToListAsync();

        if (alugueis is List<AluguelResponse> Alugueis)
        {
            return Results.Ok(Alugueis);
        }

        return Results.NotFound();
    }
}
