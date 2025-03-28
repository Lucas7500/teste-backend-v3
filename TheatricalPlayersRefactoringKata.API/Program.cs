using Microsoft.OpenApi.Models;
using TheatricalPlayersRefactoringKata.API.Controllers;
using TheatricalPlayersRefactoringKata.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRabbitMQService();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Theatrical API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddStatementEndpoints();
app.UseHttpsRedirection();
app.Run();