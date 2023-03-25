using ApiAluguelCavalos.Domain.Models;
using ApiAluguelCavalos.Domain.Services.Usuario;
using ApiAluguelCavalos.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiAluguelCavalos.Endpoints.Cavalos;

public class CavaloGet
{
    public static string Template => "/cavalo";
    public static string[] Method => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(ApplicationDbContext db)
    {
        var cavalos = await db.Cavalos.ToListAsync();
        return Results.Ok(cavalos);
    }

}
