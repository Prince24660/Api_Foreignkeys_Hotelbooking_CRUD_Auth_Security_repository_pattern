using API_Foreignkey.IReposiory;
using API_Foreignkey.IReposiory.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Test2.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionstring = builder.Configuration.GetConnectionString("cn");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionstring));
var devCorsPolicy = "devCorsPolicy";    
builder.Services.AddCors(options =>
{
    options.AddPolicy(devCorsPolicy, builder => {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddTransient<IAuthRepository, AuthRepository> ();
builder.Services.AddTransient<IStudentRepository, StudentRepository> ();
builder.Services.AddTransient<ICientRepository, ClientRepository> ();
builder.Services.AddTransient<IDBRepository, DBRepository> ();
builder.Services.AddTransient<IRoomRepository, RoomRepository> ();
builder.Services.AddTransient<IBookingRepository, BookingRepository> ();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["jwt:Audience"],
        ValidIssuer = builder.Configuration["jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"]))
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wedding Planner API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {

        Description = "Please Insert Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(
//    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(devCorsPolicy);

}

app.UseHttpsRedirection();

app.UseAuthentication ();
app.UseAuthorization();

app.MapControllers();

app.Run();
