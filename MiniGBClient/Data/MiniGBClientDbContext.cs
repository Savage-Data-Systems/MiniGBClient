using Microsoft.EntityFrameworkCore;
using MiniGBClient.Models;
using System;

namespace MiniGBClient.Data
{
    public class MiniGBClientDbContext : DbContext
    {
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        private readonly IConfiguration _configuration;
        public MiniGBClientDbContext(DbContextOptions<MiniGBClientDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_configuration.GetConnectionString("MiniGbClientDb"));
        }
    }
}
