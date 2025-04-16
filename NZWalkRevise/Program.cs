using System.Text;
using NZWalkRevise.Automapper;
using NZWalkRevise.Database;
using NZWalkRevise.Repositories.Interface;
using NZWalkRevise.Repositories.ServiceClass;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//-------Database service added by Raghvendra
builder.Services.AddDbContext<NZWalkDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionStr")));

//-------Adding Authentication Db by Raghvendra
builder.Services.AddDbContext<NzAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NzAuthConnectString")));

// Adding services to the container.
builder.Services.AddScoped<IRegion, RegionService>();
builder.Services.AddScoped<IWalk, WalkService>();
//-----Added By Raghvendra to use Automapper
builder.Services.AddAutoMapper(typeof(AutomapperClass));

//-------Setup Identity By Raghvendra
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Nzwalks")
    .AddEntityFrameworkStores<NzAuthDbContext>()
    .AddDefaultTokenProviders();

//-------Added configureation for password by Raghvendra
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

//-----Added By Raghvendra to Authentication and Authorization using JWT.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    });

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

app.MapControllers();

app.Run();
