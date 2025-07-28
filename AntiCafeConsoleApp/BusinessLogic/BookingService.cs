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
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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
    }
}
