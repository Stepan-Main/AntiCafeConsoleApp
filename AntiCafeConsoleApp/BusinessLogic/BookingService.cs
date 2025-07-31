using AntiCafeConsoleApp.DataAccess.Entity;
using AntiCafeConsoleApp.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCafeConsoleApp.BusinessLogic
{
    internal class BookingService
    {
        // Поле для юніту робочого процесу
        private readonly IUnitOfWork _unitOfWork;

        // Конструктор для ініціалізації юніту робочого процесу
        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Mетод для створення бронювання
        public async Task<Booking> CreateBookingAsync(int roomId, DateTime start, DateTime end, string customerName)
        {
            // Отримати всі доступні пакети
            var packages = await _unitOfWork.EventPackages.GetAllAsync();
            if (!packages.Any()) throw new InvalidOperationException("No event packages available");

            // Вибрати випадковий пакет
            var rnd = new Random();
            var randomPackage = packages.ElementAt(rnd.Next(packages.Count()));

            // Отрмання часу в UTC
            var utcStart = start.ToUniversalTime();
            var utcEnd = end.ToUniversalTime();

            // Створити бронювання
            var booking = new Booking
            {
                RoomId = roomId,
                StartTime = utcStart,
                EndTime = utcEnd,
                CustomerName = customerName,
                EventPackageId = randomPackage.Id
            };

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.CompleteAsync();

            // Повернути створене бронювання
            return booking;
        }

        // Mетод для видалення бронювань за ім'ям клієнта
        public async Task<int> DeleteBookingsByCustomerAsync(string customerName)
        {
            // Перевірка наявності бронювань для клієнта
            var bookingsToDelete = await _unitOfWork.Bookings
                                                      .GetByConditionAsync(b => b.CustomerName == customerName);

            // Цикл для видалення бронювань
            foreach (var booking in bookingsToDelete)
            {
                // Видалення бронювання
                _unitOfWork.Bookings.Remove(booking);
            }
            // Збереження змін в базі даних
            await _unitOfWork.CompleteAsync();
            // Повертаємо кількість видалених бронювань
            return bookingsToDelete.Count();
        }

        // Mетод для отримання бронювань за період
        public async Task<IEnumerable<Booking>> GetBookingsByPeriodAsync(DateTime start, DateTime end)
        {
            // Перетворення часу в UTC для коректного порівняння
            var utcStart = start.ToUniversalTime();
            var utcEnd = end.ToUniversalTime();

            // Отримання бронювань, які перекриваються з вказаним періодом
            return await _unitOfWork.Bookings
                                    .GetByConditionAsync(b => b.StartTime < utcEnd && b.EndTime > utcStart,
                                                         includeProperties: "Room"); // Включаємо Room для виводу її назви
        }

        // Mетод для отримання бронювань за ім'ям клієнта та періодом
        public async Task<IEnumerable<Booking>> GetBookingsByCustomerAndPeriodAsync(string customerName, DateTime start, DateTime end)
        {
            // Перетворення часу в UTC для коректного порівняння
            var utcStart = start.ToUniversalTime();
            var utcEnd = end.ToUniversalTime();

            // Отримання бронювань, які перекриваються з вказаним періодом та належать клієнту
            return await _unitOfWork.Bookings
                                    .GetByConditionAsync(b => b.CustomerName == customerName && b.StartTime < utcEnd && b.EndTime > utcStart,
                                                         includeProperties: "Room"); // Включаємо Room для виводу її назви
        }


    }
}
