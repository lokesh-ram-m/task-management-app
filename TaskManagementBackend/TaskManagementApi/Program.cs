using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TaskManagementApi.Filters;
using TaskManagementApi.Middleware;
using TaskManagementApi.Repository.Projects;
using TaskManagementApi.Repository.Tasks;
using TaskManagementApi.Repository.Users;
using TaskManagementApi.Services.Auth;
using TaskManagementApi.Services.Projects;
using TaskManagementApi.Services.Tasks;
using TaskManagementApi.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers + Validation Filter ──────────────────────────────────────────
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

// ── Dependency Injection — Repositories ──────────────────────────────────────
builder.Services.AddScoped<IUserRepository,    UserRepository>();
builder.Services.AddScoped<ITaskRepository,    TaskRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

// ── Dependency Injection — Services ──────────────────────────────────────────
builder.Services.AddScoped<IAuthService,    AuthService>();
builder.Services.AddScoped<IUserService,    UserService>();
builder.Services.AddScoped<ITaskService,    TaskService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

// ── JWT Authentication ────────────────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();

// ── Swagger ───────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── CORS — allow Angular dev server ──────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ── Middleware Pipeline ───────────────────────────────────────────────────────
// Order matters — ExceptionMiddleware must be first to catch everything
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");

// Authentication before Authorization — guard checks token first, then rooms
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
