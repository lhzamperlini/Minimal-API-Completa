using ApiAluguelCavalos.Domain.Dtos.Cavalo;
using ApiAluguelCavalos.Domain.Models;
using ApiAluguelCavalos.Domain.Services.Usuario;
using ApiAluguelCavalos.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiAluguelCavalos.Endpoints.Cavalos;

public class CavaloPost
{
    public static string Template => "/cavalo";
    public static string[] Method => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action([FromBody] CavaloRequest inputCavalo, ApplicationDbContext db)
    {
        var cavalo = new CavaloModel(inputCavalo.Nome, inputCavalo.Registro, inputCavalo.Raca, inputCavalo.Sexo, inputCavalo.Pelagem);
        await db.AddAsync(cavalo);
        await db.SaveChangesAsync();
        return Results.Created($"/cavalo/{cavalo.Id}", cavalo);
    }

}
