using System.Security.Claims;
using System.Text;
using ClinicManagementSystem.Application.Extensions;
using ClinicManagementSystem.Application.Interfaces; // <-- إضافة using للانترفيس
using ClinicManagementSystem.Infrastructure.Extensions;
using ClinicManagementSystem.Infrastructure.Identity;
using ClinicManagementSystem.WebAPI.Middleware;
using ClinicManagementSystem.WebAPI.Services;       // <-- إضافة using للـ CurrentUserService
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinic Management System API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "أدخل التوكن مباشرة بدون كلمة Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// --- تسجيل الـ CurrentUserService والـ HttpContextAccessor ---
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtOptions>(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)),
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    foreach (var roleName in Roles.All)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
    }
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var adminExists = await userManager.GetUsersInRoleAsync(Roles.Admin);
    if (adminExists.Count == 0)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "admin@clinic.com",
            Email = "admin@clinic.com",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin@1234");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();