using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using login.data.PostgresConn;
using login.jwt.config;
using login.models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferencialData.Business.business_services_implementations.Authentification;
using Platform.ReferencialData.WebAPI;
using WebApplication1.models;
using Platform.ReferentialData.DataModel.Authentification;

var builder = WebApplication.CreateBuilder(args);


//add policy
builder.Services.PermissionConfiguration();

// Add services to the container.

builder.Services.AddScoped<IAuthentificationService, AuthentificationService>();
builder.Services.AddScoped<IEmployeeManagementService, EmployeeManagementService>();
builder.Services.AddScoped<IshopownerService,shopownerService >();
builder.Services.AddScoped<ICategorie, categorieService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddEntityFrameworkNpgsql()
.AddDbContext<Postgres>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("pgsqlconnection")));


builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));



var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:secret").Value);
var TokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
    RequireExpirationTime = false,
    ValidateLifetime = true,
    ClockSkew=TimeSpan.Zero
};

builder.Services.AddCors();
builder.Services.AddSingleton(TokenValidationParameters);
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = TokenValidationParameters;
});


builder.Services.AddIdentity<employer3, AspRoles>(option => option.SignIn.RequireConfirmedEmail = true

)
.AddRoles<AspRoles>()
.AddEntityFrameworkStores<Postgres>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<RoleManager<AspRoles>>();


builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
