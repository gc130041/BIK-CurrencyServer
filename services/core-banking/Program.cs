using Microsoft.EntityFrameworkCore;
using BIK.CoreBanking.Data;
using BIK.CoreBanking.Interfaces;
using BIK.CoreBanking.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IAuditServiceClient, AuditServiceClient>();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Abrir en el CMD la carpeta BIK/services/core-banking
// Correr en la terminal:
// dotnet ef migrations add InitialCreate
// dotnet ef database update

//URL: http://localhost:5000/swagger