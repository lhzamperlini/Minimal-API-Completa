using ApiAluguelCavalos.Domain.Dtos.Cliente;
using ApiAluguelCavalos.Domain.Enums;
using ApiAluguelCavalos.Domain.Services.Usuario;
using ApiAluguelCavalos.Domain.Validations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ApiAluguelCavalos.Endpoints.Cliente;

public class ClientePost
{
    public static string Template => "/cliente/cadastrar";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(ClienteRequest clienteRequest, UserManager<IdentityUser> userManager, CriarUsuario criarUsuario)
    {
        var userClaims = new List<Claim>
        {
            new Claim("Nome", clienteRequest.Nome),
            new Claim("TipoUsuario", TipoUsuario.Cliente.ToString())
        };

        (IdentityResult identity, string userId) result = await criarUsuario.Criar(clienteRequest.Email, clienteRequest.Senha, userClaims);

        if (!result.identity.Succeeded)
            return Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());

        return Results.Created($"/cliente/{result.userId}", result.userId);
    }
}
