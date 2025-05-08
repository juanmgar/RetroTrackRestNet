using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RetroTrackRestNet.Data;

var builder = WebApplication.CreateBuilder(args);

var secretKeyBase64 = "uF0y3PI1N9e5B4FQWUKk5Jcz4OZK4IGrsQR0RpNpiX8=";
var key = Convert.FromBase64String(secretKeyBase64);

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
        ValidateAudience = false, 
        ClockSkew = TimeSpan.Zero
    };
});

// Servicios
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Gestión de Juegos",
        Description = "API REST para gestionar juegos, partidas y colecciones",
        Contact = new OpenApiContact
        {
            Name = "JuanMa Sierra García",
            Url = new Uri("https://juanmasierragarcia.eu")
        }
    });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(9095, listenOptions =>
    {
        listenOptions.UseHttps("certs/devcert.pfx", "changeit");
    });
});

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = new DataContext();
    context.Database.EnsureDeleted(); //Delete ddbb always
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gestión de Juegos v1");
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();