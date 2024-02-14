using Customer_API.Filters;
using Customer_API.ResponseTypes;
using Customer_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
    options.ReturnHttpNotAcceptable = true;
    //options.OutputFormatters.Add(new XmlDtdOutputFormatter());
    options.Filters.Add(typeof(ExceptionFilter));
})
.AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<RequestLoggerMiddleware>();

app.MapControllers();

app.Run();
