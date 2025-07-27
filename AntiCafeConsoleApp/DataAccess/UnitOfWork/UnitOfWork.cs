using AntiCafeConsoleApp.DataAccess.Entity;
using AntiCafeConsoleApp.DataAccess.Repositories;
using AntiCafeConsoleApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCafeConsoleApp.DataAccess.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRepository<Room> Rooms { get; }
        public IRepository<Activity> Activities { get; }
        public IRepository<Booking> Bookings { get; }
        public IRepository<EventPackage> EventPackages { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Rooms = new GenericRepository<Room>(context);
            Activities = new GenericRepository<Activity>(context);
            Bookings = new GenericRepository<Booking>(context);
            EventPackages = new GenericRepository<EventPackage>(context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
