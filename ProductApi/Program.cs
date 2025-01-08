using ProductApi.Daos;
using ProductApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao cont�iner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Obt�m a string de conex�o do appsettings.json.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

// Adiciona DAOs e servi�os
builder.Services.AddSingleton(new ProductDao(connectionString));
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

// Configura o pipeline de requisi��o HTTP.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
