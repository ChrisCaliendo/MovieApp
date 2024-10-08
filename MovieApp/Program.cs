using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp;
using MovieApp.Interfaces;
using MovieApp.Repositories;
using MovieApp.Models;
using Microsoft.AspNetCore.Identity;
using MovieApp.Infastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBingeRepository, BingeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddSingleton<TokenProvider>();



builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithMethods("GET", "PUT", "POST", "DELETE")
        .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
}
);

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
