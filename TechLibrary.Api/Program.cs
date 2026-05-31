using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using TechLibrary.Api.Filters;
using TechLibrary.Api.Infrastructure.Security.Tokens;

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

    // Substitui o AddSecurityRequirement global pelo filter seletivo
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // ✅ Lê de User Secrets (dev) ou variável de ambiente (produção)
        var signingKey = builder.Configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("A chave de assinatura JWT (Jwt:SigningKey) não foi configurada.");
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Checa se o emissor do token é confiável. Em um ambiente de produção, é recomendado configurar isso para true e fornecer o valor do emissor esperado.
            ValidateAudience = false, // Verifica se o token é destinado para a audiência correta. Em um ambiente de produção, é recomendado configurar isso para true e fornecer o valor da audiência esperada.
            ValidateLifetime = true, // Verifica se o token ainda é válido com base no tempo de expiração.
            ValidateIssuerSigningKey = true, // Checa se a chave de assinatura é igual a que foi usada para assinar o token.
            IssuerSigningKey = JwtSecurityKey.Generate(signingKey) // Gera a chave de segurança a partir da chave de assinatura configurada.
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
