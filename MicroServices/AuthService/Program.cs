global using Models;
global using Services;
global using Data;
global using Dtos;
global using AutoMapper;
using Microsoft.EntityFrameworkCore;

//cors variable
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

//DB context
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AUTO MAPPER
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//Services
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICryptService, CryptService>();
builder.Services.AddScoped<ICookieService, CookieService>();

//CORS POLICY
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
            {
                //authorize access from api gateway
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowCredentials();
            });
});

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


// REVIEW: This is done fore development east but shouldn't be here in production
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    // var logger = app.Services.GetService<ILogger<DataContextSeed>>();
    await context.Database.MigrateAsync();

    // await new DataContextSeed().SeedAsync(context);
    // await integEventContext.Database.MigrateAsync();
}


app.Run();