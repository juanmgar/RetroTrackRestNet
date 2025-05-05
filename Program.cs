using Microsoft.OpenApi.Models;
using System.Reflection;
using RetroTrackRestNet.Data;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();
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

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = new DataContext();
    //context.Database.EnsureDeleted(); //Delete ddbb always
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

app.UseAuthorization();
app.MapControllers();
app.Run();