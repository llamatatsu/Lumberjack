
using Microsoft.AspNetCore.StaticFiles; // Web API is an implmentation of WebApplication Initiates an instance of WebApplication
using Serilog;
using Lumberjack.API.Services;
using Microsoft.EntityFrameworkCore;
using Lumberjack.API.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Lumberjack.API.Utilities;
using Microsoft.AspNetCore.Authorization;

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/lumberjack.api..log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Ask Web App to use Serilog instead of built-in logger
builder.Host.UseSerilog();

// Add services to the container
// Built in Dependency Injection
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>(); // inject File Extension Content type provider

// Compiler directive: tells the compiler to omit or include pices of code on compile
#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

// Inject DBContext for Lumberjack Database
builder.Services.AddDbContext<LumberjackDBContext>(
    dbContextOptions => dbContextOptions.UseSqlServer(
        builder.Configuration["ConnectionStrings:LumberjackDBConnectionString"]));

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ILumberjackRepository, LumberjackRepository>();
builder.Services.AddScoped<IForkliftServiceBus, ForkliftServiceBus>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Authentication:Issuer"],
                ValidAudience = builder.Configuration["Authentication:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                                        Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
            };
        });

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApplicationAccess", policy => 
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new ApplicationAccessRequirement());
    });
});

builder.Services.AddScoped<IAuthorizationHandler, ApplicationAccessHandler>();

// Builds the WebApplication
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints => 
{
    endpoints.MapControllers();
});

// app.MapControllers(); .Net 6 format

app.Run();
