using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ApiAluguelCavalos.Domain.Services.Usuario;

public class CriarUsuario
{
    private readonly UserManager<IdentityUser> _userManager;

    public CriarUsuario(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<(IdentityResult, string)> Criar(string email, string password, List<Claim> claims)
    {
        var novoUsuario = new IdentityUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(novoUsuario, password);

        if (!result.Succeeded)
            return (result, string.Empty);

        return (await _userManager.AddClaimsAsync(novoUsuario, claims), novoUsuario.Id);
    }
}
