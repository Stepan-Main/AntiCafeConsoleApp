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
        // Поля для сервісів
        private readonly RoomService _roomService;
        private readonly BookingService _bookingService;

        // Конструктор для ініціалізації сервісів
        public ConsoleUI(RoomService roomService, BookingService bookingService)
        {
            _roomService = roomService;
            _bookingService = bookingService;
        }

        // ANSI escape codes для кольорів консолі
        public const string TitleMenuColor = "\u001b[3;30;48;5;190m"; // Синій фон, чорний текст, курсив
        public const string MenuColor = "\u001b[3;30;44m"; // Синій фон, чорний текст, курсив
        public const string ErrorColor = "\u001b[1;30;41m"; // Червоний фон, чорний текст, жирний
        public const string Reset = "\u001b[0m";            // Скинути всі атрибути

        // Головний метод для запуску інтерфейсу користувача
        public async Task RunAsync()
        {
            // Прапорець для циклу головного меню
            bool running = true;
            // Цикл головного меню
            while (running)
            {
                // Очищення консолі та виведення заголовку меню
                Console.Clear();
                // Текст та пункти меню з кольорами
                Console.WriteLine(TitleMenuColor + "------------------ Головне меню AntiCafe ---------------------" + Reset);
                Console.WriteLine(MenuColor + " 1. Керування бронюваннями 2. Огляд даних 3. Вихід з програми" + Reset);
                Console.Write("Ваш вибір: ");

                // Зчитування вибору користувача
                string? choice = Console.ReadLine();

                // Обробка вибору користувача
                switch (choice)
                {
                    case "1":
                        await ManageBookingsMenu();
                        break;
                    case "2":
                        await ReviewDataMenu();
                        break;
                    case "3":
                        running = false;
                        Console.WriteLine("Вихід з програми. До побачення!");
                        break;
                    default:
                        Console.WriteLine(ErrorColor + "Невірний вибір. Будь ласка, спробуйте ще раз." + Reset);
                        await Task.Delay(1500);
                        break;
                }
            }
        }

        // ДРУГИЙ РІВЕНЬ МЕНЮ: Керування бронюваннями
        private async Task ManageBookingsMenu()
        {
            // Прапорець для циклу підменю керування бронюваннями
            bool submenuRunning = true;
            // Цикл підменю керування бронюваннями
            while (submenuRunning)
            {
                // Очищення консолі
                Console.Clear();
                // Текст та пункти підменю з кольорами
                Console.WriteLine(TitleMenuColor + "------------------ Меню керування бронюваннями ------------------" + Reset);
                Console.WriteLine(MenuColor + " 1. Додати бронювання; 2. Видалити бронювання; 3. Повернутися" + Reset);
                Console.Write("Ваш вибір: ");

                // Зчитування вибору користувача
                string? choice = Console.ReadLine();

                // Обробка вибору користувача
                switch (choice)
                {
                    case "1":
                        await AddBooking(); // Виклик методу для додавання бронювання
                        break;
                    case "2":
                        await DeleteBookingByCustomer(); // Виклик методу для видалення бронювання
                        break;
                    case "3":
                        submenuRunning = false; // Вихід з поточного меню
                        break;
                    default:
                        Console.WriteLine(ErrorColor + "Невірний вибір. Спробуйте ще раз." + Reset);
                        break;
                }

                // Якщо підменю все ще працює, робимо паузу перед наступним циклом
                if (submenuRunning)
                {
                    Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
                    Console.ReadKey();
                }
            }
        }

        // ДРУГИЙ РІВЕНЬ МЕНЮ: Огляд даних
        private async Task ReviewDataMenu()
        {
            // Прапорець для циклу підменю огляду даних
            bool submenuRunning = true;
            // Цикл підменю огляду даних
            while (submenuRunning)
            {
                // Очищення консолі
                Console.Clear();
                // Текст та пункти підменю з кольорами
                Console.WriteLine(TitleMenuColor + "------------------ Меню огляду даних ------------------" + Reset);
                Console.WriteLine(MenuColor + " 1. Подивитися історію; 2. Список бронювань; 3. Повернутися " + Reset);
                Console.Write("Ваш вибір: ");

                // Зчитування вибору користувача
                string? choice = Console.ReadLine();

                // Обробка вибору користувача
                switch (choice)
                {
                    case "1":
                        await ViewBookingsByPeriod(); // Виклик методу для перегляду бронювань за період
                        break;
                    case "2":
                        await ViewBookingsByCustomerAndPeriod(); // Виклик методу для перегляду бронювань за автором за період
                        break;
                    case "3":
                        submenuRunning = false; // Вихід з поточного меню
                        break;
                    default:
                        Console.WriteLine(ErrorColor + "Невірний вибір. Спробуйте ще раз." + Reset);
                        break;
                }

                // Якщо підменю все ще працює, робимо паузу перед наступним циклом
                if (submenuRunning)
                {
                    Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
                    Console.ReadKey();
                }
            }
        }

        // Метод для додавання нового бронювання
        private async Task AddBooking()
        {
            // Очищення консолі та виведення заголовку додавання бронювання
            Console.Clear();
            Console.WriteLine(MenuColor + "--- Додавання нового бронювання ---" + Reset);

            // Зчитування часу початку та закінчення бронювання
            DateTime startTime = ParseDateTime("Введiть час початку (yyyy-MM-dd HH:mm): ");
            DateTime endTime = ParseDateTime("Введiть час закiнчення (yyyy-MM-dd HH:mm): ");

            // Перевірка, чи час початку не пізніший або дорівнює часу закінчення
            if (startTime >= endTime)
            {
                Console.WriteLine(ErrorColor + "Час початку не може бути пізнішим або дорівнювати часу закінчення." + Reset);
                return;
            }

            // Отримання доступних залів для вказаного проміжку часу
            var availableRooms = await _roomService.GetAvailableRooms(startTime, endTime);

            // Перевірка, чи є доступні зали
            if (!availableRooms.Any()) // Використовуємо .Any() для перевірки наявності елементів
            {
                Console.WriteLine(ErrorColor + "Немає доступних зал на вказаний проміжок часу." + Reset);
                return;
            }

            // Виведення доступних залів для вибору
            Console.WriteLine("\nДоступні зали:");
            foreach (var room in availableRooms)
            {
                Console.WriteLine($"{room.Id}. {room.Name}");
            }

            // Зчитування вибраного залу
            int roomId = ReadInteger("Виберіть зал (номер): ");

            // Перевірка, чи обраний зал є в доступних залах
            if (!availableRooms.Any(r => r.Id == roomId)) // Перевіряємо, чи обраний зал є в доступних
            {
                Console.WriteLine(ErrorColor + "Обраний зал недоступний або не існує." + Reset);
                return;
            }

            // Зчитування імені клієнта
            Console.Write("Введіть ім'я клієнта: ");
            string? customerName = Console.ReadLine();

            try
            {
                // Тут ми використовуємо BookingService для створення бронювання
                var newBooking = await _bookingService.CreateBookingAsync(roomId, startTime, endTime, customerName);
                Console.WriteLine($"\n{MenuColor}Бронювання успішно додано! ID бронювання: {newBooking.Id}{Reset}");
            }
            catch (InvalidOperationException ex) // Обробка випадку, якщо бронювання неможливе
            {
                Console.WriteLine(ErrorColor + $"Помилка при створенні бронювання: {ex.Message}" + Reset);
            }
            catch (Exception ex) // Загальна обробка інших помилок
            {
                Console.WriteLine(ErrorColor + $"Виникла неочікувана помилка: {ex.Message}" + Reset);
            }
        }

        // Реалізація функціоналу: Видалити бронювання за автором
        private async Task DeleteBookingByCustomer()
        {
            // Очищення консолі
            Console.Clear();
            // Виведення заголовку для видалення бронювання
            Console.WriteLine(MenuColor + "--- Видалення бронювання ---" + Reset);
            Console.Write("Введіть ім'я клієнта, бронювання якого потрібно видалити: ");
            // Зчитування імені клієнта
            string? customerName = Console.ReadLine();

            try
            {
                // Виклик методу сервісу для видалення бронювань за іменем клієнта
                int deletedCount = await _bookingService.DeleteBookingsByCustomerAsync(customerName);

                if (deletedCount > 0) // Перевіряємо, чи були видалені бронювання
                {
                    Console.WriteLine($"{MenuColor}Успішно видалено {deletedCount} бронювань для клієнта '{customerName}'.{Reset}");
                }
                else
                {
                    Console.WriteLine(ErrorColor + $"Бронювань для клієнта '{customerName}' не знайдено." + Reset);
                }
            }
            catch (Exception ex) // Обробка помилок при видаленні бронювань
            {
                Console.WriteLine(ErrorColor + $"Помилка при видаленні бронювання: {ex.Message}" + Reset);
            }
        }

        // Метод перегляду історії бронювань за період
        private async Task ViewBookingsByPeriod()
        {
            // Очищення консолі та виведення заголовку
            Console.Clear();
            Console.WriteLine(MenuColor + "--- Історія бронювань за період ---" + Reset);

            // Зчитування часу початку та закінчення періоду
            DateTime startTime = ParseDateTime("Введiть час початку періоду (yyyy-MM-dd HH:mm): ");
            DateTime endTime = ParseDateTime("Введiть час закінчення періоду (yyyy-MM-dd HH:mm): ");

            if (startTime >= endTime) // Перевірка, чи час початку не пізніший або дорівнює часу закінчення
            {
                Console.WriteLine(ErrorColor + "Час початку не може бути пізнішим або дорівнювати часу закінчення." + Reset);
                return;
            }

            try
            {
                // Виклик методу сервісу для отримання бронювань за період
                var bookings = await _bookingService.GetBookingsByPeriodAsync(startTime, endTime);

                if (!bookings.Any()) // Перевіряємо, чи є бронювання
                {
                    Console.WriteLine("Бронювань за вказаний період не знайдено.");
                    return;
                }

                // Виводимо знайдені бронювання
                Console.WriteLine("\n--- Знайдені бронювання ---");
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"ID: {booking.Id}, " +
                                    $"Зал: {booking.Room.Name}, " +
                                    $"Клієнт: {booking.CustomerName}, " +
                                    $"Початок: {booking.StartTime.ToLocalTime():yyyy-MM-dd HH:mm}, " +
                                    $"Кінець: {booking.EndTime.ToLocalTime():yyyy-MM-dd HH:mm}");
                }
            }
            catch (Exception ex) // Обробка помилок при отриманні бронювань
            {
                Console.WriteLine(ErrorColor + $"Помилка при отриманні бронювань: {ex.Message}" + Reset);
            }
        }

        // Метод перегляду списоку бронювань за автором за період
        private async Task ViewBookingsByCustomerAndPeriod()
        {
            // Очищення консолі та виведення заголовку
            Console.Clear();
            Console.WriteLine(MenuColor + "--- Список бронювань за автором за період ---" + Reset);

            // Зчитування імені клієнта
            Console.Write("Введіть ім'я клієнта: ");
            string? customerName = Console.ReadLine();

            // Зчитування часу початку та закінчення періоду
            DateTime startTime = ParseDateTime("Введiть час початку періоду (yyyy-MM-dd HH:mm): ");
            DateTime endTime = ParseDateTime("Введiть час закінчення періоду (yyyy-MM-dd HH:mm): ");

            if (startTime >= endTime) // Перевірка, чи час початку не пізніший або дорівнює часу закінчення
            {
                Console.WriteLine(ErrorColor + "Час початку не може бути пізнішим або дорівнювати часу закінчення." + Reset);
                return;
            }

            try
            {
                // Виклик методу сервісу для отримання бронювань за автором та період
                var bookings = await _bookingService.GetBookingsByCustomerAndPeriodAsync(customerName, startTime, endTime);

                if (!bookings.Any()) // Перевіряємо, чи є бронювання
                {
                    Console.WriteLine($"Бронювань для клієнта '{customerName}' за вказаний період не знайдено.");
                    return;
                }

                Console.WriteLine($"\n--- Знайдені бронювання для клієнта '{customerName}' ---");
                // В циклі виводимо інформацію про кожне бронювання
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"ID: {booking.Id}, " +
                                    $"Зал: {booking.Room.Name}, " +
                                    $"Початок: {booking.StartTime.ToLocalTime():yyyy-MM-dd HH:mm}, " +
                                    $"Кінець: {booking.EndTime.ToLocalTime():yyyy-MM-dd HH:mm}");
                }
            }
            catch (Exception ex) // Обробка помилок при отриманні бронювань
            {
                Console.WriteLine(ErrorColor + $"Помилка при отриманні бронювань: {ex.Message}" + Reset);
            }
        }


        // Метод для зчитування дати та часу з консолі
        private DateTime ParseDateTime(string prompt)
        {
            DateTime dateTime;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                // Дозволяємо просту дату, якщо час не вказано (тоді час буде 00:00)
                if (DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out dateTime) ||
                    DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dateTime))
                {
                    return dateTime;
                }
                Console.WriteLine(ErrorColor + "Невірний формат дати/часу. Використовуйте yyyy-MM-dd HH:mm або yyyy-MM-dd." + Reset);
            }
        }

        // Мтод для зчитування цілого числа з консолі
        private int ReadInteger(string prompt)
        {
            // Цикл для повторного запиту, поки не буде введено коректне число
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value))
                {
                    return value;
                }
                Console.WriteLine(ErrorColor + "Будь ласка, введіть числове значення." + Reset);
            }
        }
    }
}
