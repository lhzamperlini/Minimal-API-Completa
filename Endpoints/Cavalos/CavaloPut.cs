using ApiAluguelCavalos.Domain.Dtos.Cavalo;
using ApiAluguelCavalos.Domain.Models;
using ApiAluguelCavalos.Domain.Services.Usuario;
using ApiAluguelCavalos.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;

namespace ApiAluguelCavalos.Endpoints.Cavalos;

public class CavaloPut
{
    public static string Template => "/cavalo/{id}";
    public static string[] Method => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(Guid id, CavaloRequest inputCavalo, ApplicationDbContext db)
    {
        var cavalo = await db.Cavalos.FindAsync(id);

        if (cavalo is CavaloModel Cavalo)
        {
            cavalo.EditarInformacoes(inputCavalo.Nome, inputCavalo.Registro, inputCavalo.Raca, inputCavalo.Sexo, inputCavalo.Pelagem);
            await db.SaveChangesAsync();
            return Results.Ok(cavalo);
        }
        return Results.NotFound();
    }

}
