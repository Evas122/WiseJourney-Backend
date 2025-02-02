using WiseJourneyBackend.Infrastructure.Extensions;
using WiseJourneyBackend.Api.Extensions;
using WiseJourneyBackend.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocExtension();
builder.Services.AddInfrastructureExtensions(builder.Configuration);
builder.Services.AddApiExtensions(builder.Configuration);
builder.Services.AddApplicationExtensions();

builder.Services.AddRouting(options =>
options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigins");

app.MapControllers();

await app.EnsureDatabaseMigratedAsync();

app.Run();