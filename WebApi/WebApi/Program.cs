using Mock;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Services;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;//צריך???


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// הוספת CORS
var myAllowSpecificOrigin = "_myAllowSpecificOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigin,
        policy =>
        {
            policy.AllowAnyOrigin()//.WithOrigins("http://localhost:5173/") //Frontend של ה URL וודאי שזה ה
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});


//Extention -במקום לעשות פה הזרקת תלויות ניצור לכל שכבה את ההזרקת תלויות שלה
//ExtentionName(שם שכבה)
//builder.Services.AddRepository();
builder.Services.AddService();

builder.Services.AddDbContext<IContext,Database>();


//IConfiguration- ו TokenService הוספת 
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
