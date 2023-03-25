using ApiAluguelCavalos.Domain.Models;
using ApiAluguelCavalos.Infra.Data.Context;

namespace ApiAluguelCavalos.Endpoints.Cavalos;

public class CavaloDelete
{
    public static string Template => "/cavalo/{id}";
    public static string[] Method => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(Guid id, ApplicationDbContext db)
    {
        var cavalo = await db.Cavalos.FindAsync(id);
        if (cavalo is CavaloModel Cavalo)
        {
            db.Cavalos.Remove(cavalo);
            await db.SaveChangesAsync();
            return Results.Ok("Cavalo removido com sucesso!");
        }

        return Results.NotFound();

    }

}
