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
    // Класс для реалізації юніту робочого процесу, який об'єднує всі репозиторії
    internal class UnitOfWork : IUnitOfWork
    {
        // Поле для контексту бази даних
        private readonly AppDbContext _context;

        // Властивості для доступу до репозиторіїв
        public IRepository<Room> Rooms { get; }
        public IRepository<Activity> Activities { get; }
        public IRepository<Booking> Bookings { get; }
        public IRepository<EventPackage> EventPackages { get; }

        // Конструктор, який приймає контекст бази даних і ініціалізує репозиторії
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Rooms = new GenericRepository<Room>(context);
            Activities = new GenericRepository<Activity>(context);
            Bookings = new GenericRepository<Booking>(context);
            EventPackages = new GenericRepository<EventPackage>(context);
        }

        // Метод для збереження змін у базі даних
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        // Метод для звільнення ресурсів
        public void Dispose() => _context.Dispose();
    }
}
