using CyberQuiz.BLL.Interfaces;
using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Seed;
using CyberQuiz.BLL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//QuizDb
var connectionString = builder.Configuration.GetConnectionString("QuizConnection");
builder.Services.AddDbContext<QuizDbContext>(options =>
    options.UseSqlServer(connectionString));

//Identity
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<QuizDbContext>();

//Dependency injection (BLL)
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

//Kör Seeding + Migrations (Vi måste göra kopplingar först Mellan API lager och DAL)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<QuizDbContext>();

    await DbSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Identity Endpoints (Ger /login, /register osv automatiskt)
app.MapGroup("/api/identity").MapIdentityApi<IdentityUser>();

app.UseAuthorization();

app.MapControllers();

app.Run();
