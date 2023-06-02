using APICart2.Data;
using APICart2.Data.SeedData;
using APICart2.Helpers;
using APICart2.Helpers.EmailSettings;
using APICart2.Models.AuthModels;
using APICart2.Services.Identity.Concretes;
using APICart2.Services.Identity.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Text;
using APICart2.Services.ExtentionServices;
using APICart2.Services.Content.Interfaces;
using APICart2.Services.Content.Concretes;
using Microsoft.Extensions.Logging;
using APICart2.Facades;
//using Microsoft.EntityFrameworkCore.Tools;

var builder = WebApplication.CreateBuilder(args);



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString2 = builder.Configuration.GetConnectionString("IdentityConnection");
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<AppUserDbContext>(options =>
    options.UseSqlServer(connectionString2));


builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})

    .AddEntityFrameworkStores<AppUserDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddScoped<IProduct, ProductServices>();
builder.Services.AddScoped<ICategory, CategoryService>();
builder.Services.AddScoped<IShoppingCart, ShoppingCartService>();
builder.Services.AddScoped<IInvoice, InvoiceServices>();
builder.Services.AddScoped<IOrder, OrderServices>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService,EmailServices>();
builder.Services.AddScoped<OrderFaçade>();
builder.Services.AddScoped<AdminFaçade>();
builder.Services.AddScoped<IdentityFaçade>();

builder.Services
    .AddFluentEmail(emailConfig.FromEmail)
        .AddRazorRenderer()
        .AddSmtpSender(new SmtpClient()
        {
            Host = emailConfig.SmtpServer,
            Port = emailConfig.SmtpPort,
            EnableSsl = true,
            Credentials = new NetworkCredential(emailConfig.SmtpUsername, emailConfig.SmtpPassword)
        });


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(b =>
    {
        b.RequireHttpsMetadata = false;
        b.SaveToken = false;
        b.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        };
    });





// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
var app = builder.Build();

using (IServiceScope? scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = service.GetRequiredService<AppUserDbContext>();
        var userManager = service.GetRequiredService<UserManager<AppUser>>();
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        await SeedDefaultData.SeedRoles(roleManager);
        await SeedDefaultData.SeedUsers(userManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
