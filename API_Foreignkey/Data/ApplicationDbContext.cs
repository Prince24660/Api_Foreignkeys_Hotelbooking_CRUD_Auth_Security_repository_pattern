using API_Foreignkey.Models;
using Microsoft.EntityFrameworkCore;
using Test2.Models;

namespace Test2.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {
            
        }
        public DbSet <Employee>employees { get; set; }
        public DbSet <Department>departments { get; set; }
        public DbSet <Skill>skills { get; set; }
        public DbSet <EmpSkill>empSkills { get; set; }
        public DbSet <User>users { get; set; }
        public DbSet <Students>studentss { get; set; }
        public DbSet <Role>roles { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<Admin> admins { get; set; }

    }
}
