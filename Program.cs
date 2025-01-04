using AuthenticationApi.Context;
using AuthenticationApi.Interfaces;
using AuthenticationApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
	builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddDbContext<AuthAppDbContext>(options => options.UseSqlServer(
	builder.Configuration.GetConnectionString("AutApp")
));

builder.Services.AddScoped<ICryptionService, CryptionRepository>();
builder.Services.AddScoped<IUserOperationsService, UserOperationsRepository>();
builder.Services.AddScoped<IFrontendEncryption, FrontendEncryptionRepository>();

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
