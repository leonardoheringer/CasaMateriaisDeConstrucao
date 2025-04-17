    using CasaMateriaisDeConstrucao.Data;
    using CasaMateriaisDeConstrucao.Models;
    using CasaMateriaisDeConstrucao.Services;
    using Microsoft.EntityFrameworkCore;


    var builder = WebApplication.CreateBuilder(args);

    // Configuração do banco de dados (MySQL)
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

    // Serviços
    builder.Services.AddScoped<IProdutoService, ProdutoService>();
    builder.Services.AddScoped<IClienteService, ClienteService>();
    builder.Services.AddScoped<IVendaService, VendaService>();

    // Habilitar CORS para o frontend
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    app.UseCors("AllowAll");
    app.UseStaticFiles();
    app.MapFallbackToFile("index.html");

    // API Endpoints
    app.MapGet("/api/produtos", async (IProdutoService service) => 
        await service.GetAllAsync());

    app.MapGet("/api/produtos/{id}", async (int id, IProdutoService service) => 
        await service.GetByIdAsync(id));

    app.MapPost("/api/produtos", async (Produto produto, IProdutoService service) => 
    {
        await service.AddAsync(produto);
        return Results.Created($"/api/produtos/{produto.Id}", produto);
    });

    app.MapPut("/api/produtos/{id}", async (int id, Produto produto, IProdutoService service) => 
    {
        if (id != produto.Id) return Results.BadRequest();
        await service.UpdateAsync(produto);
        return Results.NoContent();
    });

    app.MapDelete("/api/produtos/{id}", async (int id, IProdutoService service) => 
    {
        await service.DeleteAsync(id);
        return Results.NoContent();
    });

    // Endpoints similares para Clientes e Vendas...

    app.Run();