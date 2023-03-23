using Microsoft.EntityFrameworkCore;
using TestCrud.Models.Domain;

namespace TestCrud.Data
{
	public class MVCDemoDBContext : DbContext
	{
       public MVCDemoDBContext(DbContextOptions options) : base(options) { }
    public DbSet<Employee> Employees { get; set; }
	}
}
