using System.Security.Claims;
using System.Text;
using EmployeeDayOffManagement.Infrastructure;
using EmployeeDayOffManagement.Core.Entities;
using EmployeeDayOffManagement.Application.Interfaces;
using EmployeeDayOffManagement.Service.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LmsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

IConfiguration configuration = builder.Configuration;

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Authority = "https://sso-sts.gjirafa.dev";
    options.RequireHttpsMetadata = false;
    options.Audience = "life_2024_api";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://sso-sts.gjirafa.dev",
        ValidAudience = configuration["AuthoritySettings:Scope"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4a9db740-2460-471a-b3a1-6d86bb99b279")),
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            context.HttpContext.User = context.Principal ?? new ClaimsPrincipal();
            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var firstName = context.HttpContext.User.FindFirst(ClaimTypes.GivenName)?.Value;
            var lastName = context.HttpContext.User.FindFirst(ClaimTypes.Surname)?.Value;
            var birthdate = context.HttpContext.User.FindFirst(ClaimTypes.DateOfBirth)?.Value;
            DateTime birthdateParsed = DateTime.Parse(birthdate);
            var userService = context.HttpContext.RequestServices.GetService<IUnitOfWork>();
            var existingUser = userService.Repository<Employee>().GetById(x => x.Id == userId).FirstOrDefault();
            if(existingUser == null)
            {
                var userToBeAdded = new Employee()
                {
                    Id = userId,
                    Firstname = firstName,
                    Lastname = lastName,
                    DateOfBirth = birthdateParsed
                };
                userService.Repository<Employee>().Create(userToBeAdded);
            }
            else
            {
                existingUser.Firstname = firstName;
                existingUser.Lastname = lastName;
                userService.Repository<Employee>().Update(existingUser);
            }
            userService.Complete();
        }
    };

    options.ForwardDefaultSelector = Selector.ForwardReferenceToken("token");
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("ManagerOnly", policy =>
        policy.RequireRole("Manager"));

    options.AddPolicy("UserOnly", policy =>
        policy.RequireRole("User"));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
        c.DefaultModelExpandDepth(0);
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Life Ecommerce");
        c.OAuthClientId("d8ce3b13-d539-4816-8d07-b1e4c7087fda");
        c.OAuthClientSecret("4a9db740-2460-471a-b3a1-6d86bb99b279");
        c.OAuthAppName("Life Ecommerce");
        c.OAuthUsePkce();
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
