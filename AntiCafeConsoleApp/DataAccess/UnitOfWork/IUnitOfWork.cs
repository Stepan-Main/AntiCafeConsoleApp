using AntiCafeConsoleApp.DataAccess.Entity;
using AntiCafeConsoleApp.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCafeConsoleApp.DataAccess.UnitOfWork
{
    // Інтерфейс для юніту робочого процесу, який об'єднує всі репозиторії
    internal interface IUnitOfWork : IDisposable
    {
        // Властивості для доступу до різних репозиторіїв
        IRepository<Room> Rooms { get; }
        IRepository<Activity> Activities { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<EventPackage> EventPackages { get; }

        // Метод для збереження змін у базі даних
        Task<int> CompleteAsync();
    }
}
