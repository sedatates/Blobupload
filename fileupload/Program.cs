using fileupload.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(provider =>
{
    return new BlobStorageHelper(builder.Configuration);
});

// Veritabanı işlemleri için ApplicationDbContext'ı ekleyin
builder.Services.AddDbContext<ApplicationDbContext>();

// Veritabanı işlemleri için FileService'ı ekleyin
builder.Services.AddScoped<FileService>();

// Veritabanı işlemleri için FileRepository'yi ekleyin
builder.Services.AddScoped<FileRepository>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();