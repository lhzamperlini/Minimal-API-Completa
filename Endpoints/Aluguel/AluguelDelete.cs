using ApiAluguelCavalos.Domain.Dtos.Aluguel;
using ApiAluguelCavalos.Domain.Models;
using ApiAluguelCavalos.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiAluguelCavalos.Endpoints.Aluguel;

public class AluguelDelete
{
    public static string Template => "/aluguel/{Id}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(Guid Id, ApplicationDbContext db, HttpContext http)
    {
        var clientId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var aluguel = await db.Alugueis.FindAsync(Id);

        if (aluguel is AluguelModel Aluguel && aluguel.ClienteId.Equals(new Guid(clientId)))
        {
            db.Alugueis.Remove(aluguel);
            db.SaveChanges();

            return Results.Ok("Aluguel Cancelado");
        }

        return Results.NotFound();
    }
}
