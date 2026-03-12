using CyberQuiz.BLL.Interfaces;
using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Seed;
using CyberQuiz.BLL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;

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

// Configure data protection to use shared key ring for local dev so the UI can forward cookies
var dpPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CyberQuiz", "keys");
Directory.CreateDirectory(dpPath);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(dpPath))
    .SetApplicationName("CyberQuizApp");

// Ensure API uses the same cookie name
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".CyberQuiz.Auth";
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
});

//Identity
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<QuizDbContext>();

//Dependency injection (BLL)
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProgressService, ProgressService>(); // ✅ Added


// AI Integration
builder.Services.AddHttpClient<IAiService, AiService>(client =>
{
    client.Timeout = TimeSpan.FromMinutes(10); // Increase to 5 minutes or as needed
});

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