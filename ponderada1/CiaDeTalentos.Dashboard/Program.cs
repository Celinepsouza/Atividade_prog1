using CiaDeTalentos.Dashboard;
using CiaDeTalentos.Dashboard.Components;
using CiaDeTalentos.Dashboard.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configuração do banco de dados PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Adicionando Blazor e Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 🔹 Adicionando suporte para Controllers e API
builder.Services.AddControllers(); // 📌 Adiciona suporte a Controllers

// 🔹 Registrando Repositórios e Serviços
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICandidatoRepository, CandidatoDataRepository>();
builder.Services.AddScoped<ICandidatoService, CandidatoService>();

// 🔹 Registrando `HttpClient` corretamente
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5088/") // ⚠ Se estiver rodando em HTTPS, altere para "https://localhost:5088/"
});

var app = builder.Build();

// 🔹 Configuração do pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// 🔹 Forçando HTTPS
app.UseHttpsRedirection();

// 🔹 Permitir acesso a arquivos estáticos
app.UseStaticFiles();
app.UseAntiforgery();

// 🔹 Mapeando Controllers diretamente (🚀 Novo padrão do .NET 7+)
app.MapControllers(); // 📌 Mapeia os Controllers automaticamente

// 🔹 Debug: Listar todas as rotas registradas
var routeEndpoints = app.Services.GetService<Microsoft.AspNetCore.Routing.EndpointDataSource>()
    ?.Endpoints
    .OfType<RouteEndpoint>()
    .Select(e => e.RoutePattern.RawText);

Console.WriteLine("📌 Rotas Registradas:");
foreach (var route in routeEndpoints ?? Enumerable.Empty<string>())
{
    Console.WriteLine($"- {route}");
}

// 🔹 Mapeando os componentes do Blazor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
