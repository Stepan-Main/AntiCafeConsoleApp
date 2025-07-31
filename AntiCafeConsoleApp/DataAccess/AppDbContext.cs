using AntiCafeConsoleApp.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCafeConsoleApp.DataAccess
{
    // Контекст бази даних для роботи з сутностями AntiCafe
    internal class AppDbContext : DbContext
    {
        // Набори даних для роботи з різними сутностями
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EventPackage> EventPackages { get; set; }

        // Конструктор, який приймає параметри для налаштування контексту
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Налаштованя зв'язку з базою даних PostgreSQL
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=AntiCafeDb;Username=postgres;Password=admin");
        }
    }
}
