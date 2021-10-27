using System;
using Employees.Models;
using EmployeesManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
