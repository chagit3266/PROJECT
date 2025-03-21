using Mock;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Database1 = Mock.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddService();

builder.Services.AddDbContext<IContext,Database1>();

//Extention -במקום לעשות פה הזרקת תלויות ניצור לכל שכבה את ההזרקת תלויות שלה
//ExtentionName(שם שכבה)
//builder.Services.AddRepository();


// הוספת IConfiguration ו-TokenService
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
