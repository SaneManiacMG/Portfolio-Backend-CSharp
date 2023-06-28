using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Backend.Csharp.Configs;
using Portfolio.Backend.Csharp.Interfaces;
using Portfolio.Backend.Csharp.Repositories;
using Portfolio.Backend.Csharp.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddPolicy(name: "PortfolioFrontend",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    }));

// Add services to the container.
builder.Services.AddSingleton<IAuthorizationHandler, RoleRequirementHandler>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// jwt authentication
var key = "SaneManiacWorksSecurityOchoMuerte90";
var tokenKey = Encoding.ASCII.GetBytes(key);

builder.Services.AddSingleton<CustomJwtBearerEvents>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
        ValidateIssuer = true,
        ValidIssuer = "SaneManiacWorks",
        ValidateAudience = true,
        ValidAudience = "PortfolioFrontend",
    };

    x.EventsType = typeof(CustomJwtBearerEvents);
});


builder.Services.AddSingleton<JwtAuthenticationManager>(new JwtAuthenticationManager(tokenKey));

// dependency injection here
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISequenceGenerator, SequenceGenerator>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();

// automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// building db context
/*builder.Services.AddDbContext<PortfolioDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("PortfolioApiConnectionString")));*/

// build db context using Postgres
builder.Services.AddDbContext<PortfolioDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("PortfolioApiConnectionString");
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(60);
        npgsqlOptions.EnableRetryOnFailure(3);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PortfolioFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
