using Customer_API.Filters;
using Customer_API.ResponseTypes;
using Customer_API.Services;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var JwtSecurityToken = builder.Configuration.GetSection("JwtSecurityToken");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
   options.TokenValidationParameters = new TokenValidationParameters
   {
       ValidateIssuer = true,
       ValidateAudience = true,
       ValidateLifetime = true,
       ValidateIssuerSigningKey = true,
       ValidIssuer = JwtSecurityToken.GetSection("Issuer").Value,
       ValidAudience = JwtSecurityToken.GetSection("Audience").Value,
       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecurityToken.GetSection("Key")?.Value)),
       ClockSkew = TimeSpan.Zero,
       SaveSigninToken = true,
   };
   options.SaveToken = true;
});


builder.Services.AddScoped<IServicesBAL, ServicesBAL>();
builder.Services.AddScoped<IOrderServiceBAL, OrderServiceBAL>();
builder.Services.AddSingleton<TokenValidationParameters>(provider =>
{
    return new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecurityToken.GetSection("Key")?.Value)),// Your issuer signing key,
        ValidateIssuer = true,
        ValidIssuer = JwtSecurityToken.GetSection("Issuer").Value, // Your valid issuer,
        ValidateAudience = true,
        ValidAudience = JwtSecurityToken.GetSection("Audience").Value,// Your valid audience,
        ValidateLifetime = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.ReturnHttpNotAcceptable = false;
    //options.OutputFormatters.Add(new XmlDtdOutputFormatter());
    options.Filters.Add(typeof(ExceptionFilter));
});
//.AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});
builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
}).AddEntityFrameworkStores<CustomerDbContext>()
  .AddDefaultTokenProviders(); ;
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<RequestLoggerMiddleware>();

app.MapControllers();

app.Run();
