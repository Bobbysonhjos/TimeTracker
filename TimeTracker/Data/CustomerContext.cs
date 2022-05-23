using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Entities;



namespace TimeTracker.Data
{
    public class CustomerContext : DbContext
    {
        public DbSet<Project>Projects { get; set; }
        public DbSet<TimeTracking>TimeTracking { get; set; }
        public DbSet<Customer> Customers { get;  set; }

        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {

        }

    }
}
