using ApiAluguelCavalos.Domain.Services.Usuario;
using ApiAluguelCavalos.Endpoints.Aluguel;
using ApiAluguelCavalos.Endpoints.Cavalos;
using ApiAluguelCavalos.Endpoints.Cliente;
using ApiAluguelCavalos.Endpoints.Funcionario;
using ApiAluguelCavalos.Endpoints.Login;
using ApiAluguelCavalos.Infra.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionString:CavaloDb"]);
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    //Configurando seguran�a de senha
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

//Configura��o de Autentica��o JWT
builder.Services.AddAuthorization(options =>
{
    //Configura��o de Necessidade de Autoriza��o para todos os Endpoints
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("Cliente", p => p.RequireAuthenticatedUser().RequireClaim("TipoUsuario", "Cliente"));
    options.AddPolicy("Funcionario", p => p.RequireAuthenticatedUser().RequireClaim("TipoUsuario", "Funcionario"));
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"]))
    };
});


// Adicionando servi�os
//builder.Services.AddScoped<QueryAllUsersWithClaimName>();
builder.Services.AddScoped<CriarUsuario>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    //Configura��o do Swagger para Bearer Token
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
// App � aquilo que ele est� usando;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

//Adicionar Mapeamento de Metodos
app.MapMethods(FuncionarioPost.Template, FuncionarioPost.Methods, FuncionarioPost.Handle).AllowAnonymous();
app.MapMethods(ClientePost.Template, ClientePost.Methods, ClientePost.Handle).AllowAnonymous();
app.MapMethods(LoginPost.Template, LoginPost.Methods, LoginPost.Handle).AllowAnonymous();

//CavaloMetodos
app.MapMethods(CavaloGet.Template, CavaloGet.Method, CavaloGet.Handle).AllowAnonymous();
app.MapMethods(CavaloPost.Template, CavaloPost.Method, CavaloPost.Handle).RequireAuthorization("Funcionario");
app.MapMethods(CavaloPut.Template, CavaloPut.Method, CavaloPut.Handle).RequireAuthorization("Funcionario");
app.MapMethods(CavaloDelete.Template, CavaloDelete.Method, CavaloDelete.Handle).RequireAuthorization("Funcionario");

//AluguelMetodos
app.MapMethods(AluguelGet.Template, AluguelGet.Methods, AluguelGet.Handle).RequireAuthorization("Cliente");
app.MapMethods(AluguelPost.Template, AluguelPost.Methods, AluguelPost.Handle).RequireAuthorization("Cliente");
app.MapMethods(AluguelPut.Template, AluguelPut.Methods, AluguelPut.Handle).RequireAuthorization("Cliente");
app.MapMethods(AluguelDelete.Template, AluguelDelete.Methods, AluguelDelete.Handle).RequireAuthorization("Cliente");


app.Run();
