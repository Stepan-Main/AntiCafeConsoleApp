using AntiCafeConsoleApp.BusinessLogic.DTO;
using AntiCafeConsoleApp.DataAccess.Entity;
using AntiCafeConsoleApp.DataAccess.UnitOfWork;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCafeConsoleApp.BusinessLogic
{
    internal class RoomService
    {
        // Сервіс для роботи з кімнатами
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        // Конструктор для ініціалізації юніту робочого процесу та мапера
        public RoomService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // Метод для отримання доступних кімнат в заданому часовому проміжку
        public async Task<IEnumerable<RoomDto>> GetAvailableRooms(DateTime start, DateTime end)
        {
            // Гарантуємо, що час початку і закінчення вказані в UTC
            var utcStart = start.ToUniversalTime();
            var utcEnd = end.ToUniversalTime();

            var allRooms = await _uow.Rooms.GetAllAsync();
            var bookings = await _uow.Bookings.GetAllAsync();

            Console.WriteLine($"Start: {start}, Finish: {end}");

            // Перевірка наявності кімнат
            var busyRoomIds = bookings
                .Where(b =>
                {
                    // Нормалізуємо час з БД до UTC для порівняння.
                    DateTime bookingStartUtc = b.StartTime.ToUniversalTime();
                    DateTime bookingEndUtc = b.EndTime.ToUniversalTime();

                    // Перевіряємо, чи перетинається час бронювання з запитом на вільні кімнати.
                    bool overlaps = utcStart < bookingEndUtc && utcEnd > bookingStartUtc;
                    return overlaps; // Повертаємо true, якщо бронювання перетинається з запитом.
                })
                .Select(b => b.RoomId) // Отримуємо ID кімнат, які зайняті в цей час
                .Distinct(); // Видаляємо дублікати ID кімнат

            // Виводимо кількість зайнятих кімнат
            Console.WriteLine($"Bookings count: {busyRoomIds.Count()}");

            // Фільтруємо всі кімнати, залишаючи тільки ті, які не зайняті в цей час
            var availableRooms = allRooms
                .Where(r => !busyRoomIds.Contains(r.Id));

            // Виводимо кількість доступних кімнат
            Console.WriteLine($"Available rooms count: {availableRooms.Count()}");

            // Повертаємо доступні кімнати, мапуючи їх на DTO
            return _mapper.Map<IEnumerable<RoomDto>>(availableRooms);
        }
    }
}
