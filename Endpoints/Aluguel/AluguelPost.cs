using ApiAluguelCavalos.Domain.Dtos.Aluguel;
using ApiAluguelCavalos.Domain.Models;
using ApiAluguelCavalos.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiAluguelCavalos.Endpoints.Aluguel;

public class AluguelPost
{
    public static string Template => "/aluguel";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action([FromBody] AluguelRequest inputAluguel, ApplicationDbContext db, UserManager<IdentityUser> userManager, HttpContext http)
    {
        var clienteId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var cliente = await userManager.FindByIdAsync(clienteId);
        var cavalo = await db.Cavalos.FindAsync(inputAluguel.CavaloId);
        
        if (cavalo is CavaloModel Cavalo && cliente is IdentityUser Usuario)
        {
            var aluguel = new AluguelModel(new Guid(clienteId), inputAluguel.DataReserva, inputAluguel.NumeroHoras, inputAluguel.CavaloId)
            {
                Cavalo = cavalo,
                Cliente = cliente
            };
            await db.Alugueis.AddAsync(aluguel);
            db.SaveChanges();

            var response = new AluguelResponse(aluguel.Id, aluguel.CavaloId, aluguel.ClienteId, cliente.Email, cavalo.Nome, aluguel.NumeroHoras, aluguel.DataReserva);
            
            return Results.Created($"/aluguel/{aluguel.Id}", response);
        }

        return Results.NotFound();
    }
}
