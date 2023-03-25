using ApiAluguelCavalos.Domain.Dtos.Funcionario;
using ApiAluguelCavalos.Domain.Enums;
using ApiAluguelCavalos.Domain.Services.Usuario;
using ApiAluguelCavalos.Domain.Validations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ApiAluguelCavalos.Endpoints.Funcionario;

public class FuncionarioPost
{
    public static string Template => "/funcionario/cadastrar";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(FuncionarioRequest funcionarioRequest, UserManager<IdentityUser> userManager, CriarUsuario criarUsuario)
    {
        var userClaims = new List<Claim>()
        {
            new Claim("CodigoFuncionario", funcionarioRequest.CodigoFuncionario),
            new Claim("Nome", funcionarioRequest.Nome),
            new Claim("TipoUsuario", TipoUsuario.Funcionario.ToString())
        };

        (IdentityResult identity, string userId) result = await criarUsuario.Criar(funcionarioRequest.Email, funcionarioRequest.Senha, userClaims);
        if (!result.identity.Succeeded)
            return Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());

        return Results.Created($"/funcionario/{result.userId}", result.userId);
    }

}
