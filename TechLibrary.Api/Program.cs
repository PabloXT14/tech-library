using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using TechLibrary.Api.Filters;

const string AUTHENTICATION_TYPE = "Bearer";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = AUTHENTICATION_TYPE,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Insira um token JWT válido para se autenticar.",
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(AUTHENTICATION_TYPE, document)] = []
    });
});

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Checa se o emissor do token é confiável. Em um ambiente de produção, é recomendado configurar isso para true e fornecer o valor do emissor esperado.
            ValidateAudience = false, // Verifica se o token é destinado para a audiência correta. Em um ambiente de produção, é recomendado configurar isso para true e fornecer o valor da audiência esperada.
            ValidateLifetime = true, // Verifica se o token ainda é válido com base no tempo de expiração.
            ValidateIssuerSigningKey = true, // Checa se a chave de assinatura é igual a que foi usada para assinar o token.
            IssuerSigningKey = SecurityKey()
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();

    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


SymmetricSecurityKey SecurityKey()
{
    var signingKey = "uvoKT4tfbS6Ix8rTKJt23hfqlrhT3zTr"; // OBS: em produção salve essa key como variável de ambiente (mínimo de 32 caracteres)
        
    var symmetricKey = Encoding.UTF8.GetBytes(signingKey);
        
    return new SymmetricSecurityKey(symmetricKey);
}