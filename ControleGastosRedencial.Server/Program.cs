// Inicialização e configuração do backend ASP.NET Core para o projeto Controle de Gastos Residencial.
using ControleGastosRedencial.Server;
using ControleGastosRedencial.Server.Interfaces;
using ControleGastosRedencial.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração de Controllers e opções de serialização JSON.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Swagger/OpenAPI para documentação dos endpoints.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do SQLite (string de conexão).
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = "Data Source=app.db";
}

// Configurar DbContext e opções específicas para desenvolvimento.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString);

    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
    }
});

// Registra os repositórios de Pessoa, Categoria e Transação.
builder.Services.AddScoped<IRepositoryPessoa, RepositoryPessoa>();
builder.Services.AddScoped<IRepositoryCategoria, RepositoryCategoria>();
builder.Services.AddScoped<IRepositoryTransacao, RepositoryTransacao>();

// Configuração de CORS para permitir chamadas do frontend React.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "http://localhost:5173", "https://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Inicialização do banco de dados: cria e valida estrutura básica.
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Criar o banco de dados se não existir.
    context.Database.EnsureCreated();

    Console.WriteLine("Banco de dados SQLite inicializado com sucesso!");
    Console.WriteLine($"Local do arquivo: {Path.GetFullPath("app.db")}");

    // Verificar se a tabela foi criada corretamente
    var tableInfo = await context.Database.SqlQueryRaw<string>(
        "SELECT name FROM sqlite_master WHERE type='table' AND name='Pessoas'").ToListAsync();

    if (tableInfo.Any())
    {
        Console.WriteLine("Tabela 'Pessoas' criada com sucesso!");

        // Verificar estrutura da tabela
        var columns = await context.Database.SqlQueryRaw<string>(
            "PRAGMA table_info(Pessoas)").ToListAsync();
        Console.WriteLine("Estrutura da tabela Pessoas:");
        foreach (var column in columns)
        {
            Console.WriteLine($"   {column}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao inicializar o banco de dados: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Pipeline HTTP e configurações específicas para desenvolvimento.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowReactApp");

    // Middleware para tratamento de erros detalhados
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

// Endpoint de Health Check: retorna estado básico da aplicação e do banco.
app.MapGet("/api/health", async (ApplicationDbContext db) =>
{
    try
    {
        var canConnect = await db.Database.CanConnectAsync();
        var pessoasCount = await db.Pessoas.CountAsync();

        return Results.Json(new
        {
            status = "Healthy",
            database = "SQLite",
            connected = canConnect,
            pessoasCount = pessoasCount,
            timestamp = DateTime.UtcNow
        });
    }
    catch
    {
        return Results.Json(new
        {
            status = "Unhealthy",
            database = "Connection Failed",
            timestamp = DateTime.UtcNow
        }, statusCode: 503);
    }
});

app.Run();
