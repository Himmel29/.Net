using Microsoft.EntityFrameworkCore;
using Repo_UnitofWork.Models;

namespace Repo_UnitofWork.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor nhận các tùy chọn DbContext, sử dụng base để truyền các tham số
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        

        // DbSet cho bảng Student
        public DbSet<Student> Students { get; set; }
    }
}
