using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webapi_blazor.models.EbayDB;

public class JwtAuthService
{
    private readonly string? _key;
    private readonly string? _issuer;
    private readonly string? _audience;
    private readonly EbayContext _context;
    public JwtAuthService(IConfiguration Configuration, EbayContext db)
    {
        _key = Configuration["jwt:Serect-Key"];
        _issuer = Configuration["jwt:Issuer"];
        _audience = Configuration["jwt:Audience"];
        _context = db;
    }
    
    public string GenerateToken(webapi_blazor.models.EbayDB.User userLogin)
    {
        // Khóa bí mật để ký token
        var key = Encoding.ASCII.GetBytes(_key);

         //Add role vào token 
      
        // Tạo danh sách các claims cho token
        var claims = new List<Claim>
        {
            new Claim("UserName", userLogin.Username),               // Claim mặc định cho username
            // new Claim(ClaimTypes.Role, userLogin.Role),                   // Claim mặc định cho Role
            new Claim(JwtRegisteredClaimNames.Sub, userLogin.Username),   // Subject của token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID của token
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), // Thời gian tạo token
            new Claim(JwtRegisteredClaimNames.Email, userLogin.Email), // Thời gian tạo token
       
        };
        //   List<string> lstRole = new List<string>();
        // var userRoles = _context.Database.SqlQueryRaw<string>($@"SELECT Roles.RoleName FROM Roles, Users, UserRole where UserRole.UserId = Users.id and Roles.Id = UserRole.RoleId and Users.Id = {userLogin.Id}").ToList();
        // foreach(var item in userRoles){
        //     claims.Add(new Claim(ClaimTypes.Role, item));
        // }
        var userRoles = _context.UserRoles.Include(n=>n.Role).Where(us => us.UserId == userLogin.Id).ToList();
        foreach(var item in userRoles){
            claims.Add(new Claim(ClaimTypes.Role, item.Role.RoleName));
        }


        // Tạo khóa bí mật để ký token
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );
        // Thiết lập thông tin cho token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2), // Token hết hạn sau 1 giờ
            SigningCredentials = credentials,
            Issuer = _issuer,                 // Thêm Issuer vào token
            Audience = _audience,              // Thêm Audience vào token
        };
        // Tạo token bằng JwtSecurityTokenHandler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Trả về chuỗi token đã mã hóa
        return tokenHandler.WriteToken(token);
    }

    public string DecodePayloadToken(string token)
    {
        try
        {
            // Kiểm tra token có null hoặc rỗng không
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token không được để trống", nameof(token));
            }

            // Tạo handler và đọc token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Lấy username từ claims (thường nằm trong claim "sub" hoặc "name")
            var usernameClaim = jwtToken.Claims.FirstOrDefault(x =>x.Type == "UserName"); // Common in some identity providers

            if (usernameClaim == null)
            {
                throw new InvalidOperationException("Không tìm thấy username trong payload");
            }

            return usernameClaim.Value;
        }
        catch (Exception ex)
        {
            // Xử lý lỗi (có thể log lỗi ở đây)
            throw new InvalidOperationException($"Lỗi khi decode token: {ex.Message}", ex);
        }
    }

}