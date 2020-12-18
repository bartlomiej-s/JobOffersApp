using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobOffer> JobOfers { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}