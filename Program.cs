using Microsoft.EntityFrameworkCore;
using webapi_blazor.models.EbayDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Đọc connection string từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("EbayConnection");
//Kết nối db
builder.Services.AddDbContext<EbayContext>(options =>options.UseSqlServer(connectionString));
//Kết nối db 2 
// builder.Services.AddDbContext<EbayContextExtend>(options =>options.UseSqlServer(connectionString));
//DI service Auto mapper
builder.Services.AddAutoMapper(typeof(Program));






//-----------------------------------------------------------------------------


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
//Cấu hình blazor server
app.UseStaticFiles();
app.MapBlazorHub();
app.UseRouting();
app.MapFallbackToPage("/_Host");


app.Run();


