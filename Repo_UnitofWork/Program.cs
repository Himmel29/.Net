using Microsoft.EntityFrameworkCore;
using Repo_UnitofWork.Data;
using Repo_UnitofWork.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext để sử dụng SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Đăng ký Repository
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Cấu hình Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    // Cấu hình cookie hết hạn sau một khoảng thời gian ngắn
    options.ExpireTimeSpan = TimeSpan.FromMinutes(1); // Thời gian cookie hết hạn
    options.SlidingExpiration = true; // Gia hạn cookie nếu người dùng vẫn đang hoạt động
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    googleOptions.CallbackPath = "/signin-google";
});


// Thêm dịch vụ Controller và Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình middleware cho HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");  // Xử lý lỗi
    app.UseHsts();  // Cấu hình HSTS cho môi trường sản xuất

}

// Sửa route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}");

app.UseHttpsRedirection();  // Chuyển hướng HTTP sang HTTPS
app.UseStaticFiles();  // Dùng để phục vụ các tệp tĩnh

app.UseRouting();  // Cấu hình định tuyến
app.UseEndpoints(endpoints =>
{
    // Cấu hình route cho Student
    endpoints.MapControllerRoute(
        name: "student",
        pattern: "{controller=Student}/{action=Create}/{id?}");
   


});
app.UseAuthentication();
app.UseAuthorization();  // Cấu hình xác thực và phân quyền

// Chạy ứng dụng
app.Run();
