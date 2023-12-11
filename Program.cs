using AsientosContrablesApi.Repository;
using AsientosContrablesApi.Services;
using Microsoft.EntityFrameworkCore;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();

builder.Services.AddDbContext<Context>(options =>
{
  options.UseSqlServer(connectionString);
});

builder.Services.AddLogging();

builder.Services.AddSingleton<ISAPService, SAPService>();

builder.Services.AddScoped<IContabilidadRepository, ContabilidadRepository>();

builder.Services.AddHttpClient("SAPApi", client =>
{
  client.BaseAddress = new Uri("http://200.73.224.11:4003/");
  client.DefaultRequestHeaders.Add("Company", "DEMO");

});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
