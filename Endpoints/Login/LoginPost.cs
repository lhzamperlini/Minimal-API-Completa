using ApiAluguelCavalos.Domain.Dtos.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiAluguelCavalos.Endpoints.Login;

public class LoginPost
{
    public static string Template => "/login";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(LoginRequest loginRequest, UserManager<IdentityUser> userManager, IConfiguration configuration, IWebHostEnvironment enviroment)
    {
        var usuario = await userManager.FindByEmailAsync(loginRequest.Email);
        if(usuario == null)
            return Results.BadRequest();

        if(!userManager.CheckPasswordAsync(usuario, loginRequest.Senha).Result)
            return Results.BadRequest();

        var claims = await userManager.GetClaimsAsync(usuario);
        var subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, loginRequest.Email),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id)
        });

        subject.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"],
            // Alterando expiração do token de acordo com o ambiente
            Expires = enviroment.IsDevelopment() || enviroment.IsStaging() ? DateTime.UtcNow.AddHours(2) : DateTime.UtcNow.AddHours(2)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Results.Ok(new
        {
            token = tokenHandler.WriteToken(token)
        });
    }
}
