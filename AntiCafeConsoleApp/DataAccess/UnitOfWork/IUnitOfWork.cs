using AntiCafeConsoleApp.DataAccess.Entity;
using AntiCafeConsoleApp.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCafeConsoleApp.DataAccess.UnitOfWork
{
    internal interface IUnitOfWork : IDisposable
    {
        IRepository<Room> Rooms { get; }
        IRepository<Activity> Activities { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<EventPackage> EventPackages { get; }
        Task<int> CompleteAsync();
    }
}
