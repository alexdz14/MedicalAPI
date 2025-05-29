using MedicalAPI.Models;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración de JWT
var key = Encoding.ASCII.GetBytes("SuperUltraClaveSeguraJWT1234567890"); 

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Agregar configuración de ClinicaDbSettings
builder.Services.Configure<ClinicaDbSettings>(
    builder.Configuration.GetSection("ClinicaDbSettings"));

// Registrar servicios personalizados
builder.Services.AddSingleton<MongoService>();
builder.Services.AddSingleton<UsuarioService>();

//Servicio de citas
builder.Services.AddSingleton<CitaService>();

//Servicio de consulta
builder.Services.AddSingleton<ConsultaService>();

// Servicios base
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<LogService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();


