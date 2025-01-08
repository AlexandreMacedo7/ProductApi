using ProductApi.Daos;
using ProductApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Obtém a string de conexão do appsettings.json.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

// Adiciona DAOs e serviços
builder.Services.AddSingleton(new ProductDao(connectionString));
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

// Configura o pipeline de requisição HTTP.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
