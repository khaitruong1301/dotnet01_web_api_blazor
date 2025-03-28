using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using webapi_blazor.Filter;
using webapi_blazor.Helper;
using webapi_blazor.models.EbayDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Bật giao diện authentication 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    // 🔥 Thêm hỗ trợ Authorization header tất cả api
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token vào ô bên dưới theo định dạng: Bearer {token}"
    });

    // 🔥 Định nghĩa yêu cầu sử dụng Authorization trên từng api
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});



builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Đọc connection string từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("EbayConnection");
//Kết nối db
builder.Services.AddDbContext<EbayContext>(options => options.UseLazyLoadingProxies(false).UseSqlServer(connectionString));
//Kết nối db 2 
// builder.Services.AddDbContext<EbayContextExtend>(options =>options.UseSqlServer(connectionString));
//DI service Auto mapper
builder.Services.AddAutoMapper(typeof(Program));

//DI Service JWT
builder.Services.AddScoped<JwtAuthService>();

// middleware cross
builder.Services.AddCors(option=>{
    option.AddPolicy("allow_origin", policy => {
        // policy.AllowAnyOrigin(); //Cho phép tất cả các client đều có thể gửi dữ liệu đến server
        policy.WithOrigins("https://localhost:5001","https://login.cybersoft.edu.vn","http://127.0.0.1:5500")
            .AllowAnyHeader() //Cho phép rq tất cả header
            .AllowAnyMethod() //Cho phép rq tất cả method (POST,PUT,GET,DELETE,OPTION)
            .AllowCredentials(); ////Cho phép cookie...
    });
    // option.AddDefaultPolicy();
      option.AddPolicy("allow_GET", policy => {
        // policy.AllowAnyOrigin(); //Cho phép tất cả các client đều có thể gửi dữ liệu đến server
        policy.WithOrigins("https://localhost:5001","https://login.cybersoft.edu.vn","http://127.0.0.1:5500")
            .AllowAnyHeader() //Cho phép rq tất cả header
            .WithMethods("GET") //Cho phép rq tất cả method (POST,PUT,GET,DELETE,OPTION)
            .AllowCredentials(); ////Cho phép cookie...
    });

});
// Cấu hình authentication của jwt
//Thêm middleware authentication
var privateKey = builder.Configuration["jwt:Serect-Key"];
var Issuer = builder.Configuration["jwt:Issuer"];
var Audience = builder.Configuration["jwt:Audience"];
// Thêm dịch vụ Authentication vào ứng dụng, sử dụng JWT Bearer làm phương thức xác thực
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{        
        // Thiết lập các tham số xác thực token
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            // Kiểm tra và xác nhận Issuer (nguồn phát hành token)
            ValidateIssuer = true, 
            ValidIssuer = Issuer, // Biến `Issuer` chứa giá trị của Issuer hợp lệ
            // Kiểm tra và xác nhận Audience (đối tượng nhận token)
            ValidateAudience = true,
            ValidAudience = Audience, // Biến `Audience` chứa giá trị của Audience hợp lệ
            // Kiểm tra và xác nhận khóa bí mật được sử dụng để ký token
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)), 
            // Sử dụng khóa bí mật (`privateKey`) để tạo SymmetricSecurityKey nhằm xác thực chữ ký của token
            // Giảm độ trễ (skew time) của token xuống 0, đảm bảo token hết hạn chính xác
            ClockSkew = TimeSpan.Zero, 
            // Xác định claim chứa vai trò của user (để phân quyền)
            RoleClaimType = ClaimTypes.Role, 
            // Xác định claim chứa tên của user
            NameClaimType = ClaimTypes.Name, 
            // Kiểm tra thời gian hết hạn của token, không cho phép sử dụng token hết hạn
            ValidateLifetime = true
        };
});
// Thêm dịch vụ Authorization để hỗ trợ phân quyền người dùng
builder.Services.AddAuthorization();

//filter
builder.Services.AddScoped<Authorize_Thuan>();
builder.Services.AddScoped<LogFilter>();
builder.Services.AddScoped<FilterDemoAsync>();
// cache
builder.Services.AddMemoryCache();

//-----------------------------------------------------------------------------


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//middleware
//Cấu hình middleware error 
// app.UseExceptionHandler(appBuilder =>
// {
//     appBuilder.Run(async context =>
//     {
//         var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
//         // context.Request["Request URL"]
//         // Đặt response content-type thành JSON
//         context.Response.ContentType = "application/json";
//         context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//         // Trả về JSON chứa thông tin lỗi
//         var errorResponse = new { messageError = exceptionFeature?.Error.Message ?? "Lỗi không xác định!" , StatusCode = context.Response.StatusCode};
//         //Tạo ra view model tương ứng 
//         await context.Response.WriteAsJsonAsync(errorResponse);
//     });
// });
//Xác thực
app.UseRouting(); //routing của blazor server và api
//middle ware cors

app.UseCors("allow_origin");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers(); //vào api 

//Cấu hình blazor server
app.UseStaticFiles(); //middle ware của tệp tĩnh (mặc định là wwwroot)
app.MapBlazorHub();

app.MapFallbackToPage("/_Host");


app.Run();


