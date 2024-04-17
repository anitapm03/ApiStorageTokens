using ApiStorageTokens.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ServiceSasToken>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

//aqui se van mapeando los distintos metodos necesarios
app.MapGet("/testApi", () =>
{
    return "Testing Minimal Api";
});

//queremos un metodo para generar el token y que reciba un curso logicamente
//pero no podemos usar las palabras reservadas [action] o [controller]
//necesitamos el service y hay 2 formas de recuperarlo
//1. dentro del metodo buscando el service con Services.GetService<>
//2. usando la inyección
app.MapGet("/token/{curso}", async (string curso, ServiceSasToken service) =>
{
    string token = service.GenerateToken(curso);
    return new { token = token };
});

app.Run();

