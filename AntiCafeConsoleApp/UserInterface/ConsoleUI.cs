using AntiCafeConsoleApp.BusinessLogic;
using AntiCafeConsoleApp.BusinessLogic.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCafeConsoleApp.UserInterface
{
    internal class ConsoleUI
    {
        private readonly RoomService _roomService;
        private readonly BookingService _bookingService;

        public ConsoleUI(RoomService roomService, BookingService bookingService)
        {
            _roomService = roomService;
            _bookingService = bookingService;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Введiть час початку (yyyy-MM-dd HH:mm):");
            var start = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Введiть час закiнчення (yyyy-MM-dd HH:mm):");
            var end = DateTime.Parse(Console.ReadLine());

            var availableRooms = (await _roomService.GetAvailableRooms(start, end)).ToList();
            if (!availableRooms.Any())
            {
                Console.WriteLine("Немає доступних зал.");
                return;
            }

            Console.WriteLine("Доступні зали:");
            for (int i = 0; i < availableRooms.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableRooms[i].Name}");
            }

            Console.WriteLine("Виберіть зал (номер):");
            var index = int.Parse(Console.ReadLine()!) - 1;
            var selectedRoom = availableRooms[index];

            Console.WriteLine("Введіть ім’я клієнта:");
            var customerName = Console.ReadLine()!;

            var booking = await _bookingService.CreateBookingAsync(selectedRoom.Id, start, end, customerName);

            Console.WriteLine($"Бронювання створено: {booking.CustomerName}, кімната {selectedRoom.Name}, подія: {booking.EventPackage.Title}");
        }
    }
}
