using FluentAssertions.Common;
using GraduationProject.Data;
using GraduationProject.Data.Models;
using GraduationProject.Data.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NETCore.MailKit.Core;
using Newtonsoft.Json;
using System.Text;
using UserManagmentService.Models;
using UserManagmentService.Services;
using EmailService = UserManagmentService.Services.EmailService;
using IEmailService = UserManagmentService.Services.IEmailService;


var builder = WebApplication.CreateBuilder(args);
var configration = builder.Configuration;
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(configration.GetConnectionString("MyConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Tokens.PasswordResetTokenProvider = ResetPasswordTokenProvider.ProviderKey;
}).AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders().AddTokenProvider<ResetPasswordTokenProvider>(ResetPasswordTokenProvider.ProviderKey);
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });
//builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders() ;

   
builder.Services.Configure<IdentityOptions>(options =>options.SignIn.RequireConfirmedEmail=true);
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>options.TokenLifespan=TimeSpan.FromHours(10));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configration["JWT:ValidAudience"],
        ValidIssuer = configration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configration["JWT:SecretKey"]))
};
});
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});



var emailConfig = configration.GetSection("EmailConfigration").Get<EmailConfigration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddControllers().AddNewtonsoftJson();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In=ParameterLocation.Header,
        Description="Please Enter A Valid Token",
        Name="Authorization",
        Type=SecuritySchemeType.Http,
        BearerFormat="JWT",
        Scheme="Bearer"

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
