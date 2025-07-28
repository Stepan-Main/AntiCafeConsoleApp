using AntiCafeConsoleApp.BusinessLogic;
using AntiCafeConsoleApp.DataAccess;
using AntiCafeConsoleApp.DataAccess.Entity;
using AntiCafeConsoleApp.DataAccess.UnitOfWork;
using AntiCafeConsoleApp.UserInterface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.Common;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        // Налаштування кодування консолі для підтримки українських символів
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var dbContext = new AppDbContext();

        // Перевірка підключення до бази даних і наявність у ній данних
        try
        {
            await dbContext.Database.CanConnectAsync();
            Console.WriteLine("Здійснено коректне пі=дключення до БД!");
            var roomsCount = await dbContext.Rooms.CountAsync();
            Console.WriteLine($"Кільлість кімнат в БД: {roomsCount}");
            var bookingsCountInDb = await dbContext.Bookings.CountAsync();
            Console.WriteLine($"Кількість бронювань в БД: {bookingsCountInDb}");
            await Task.Delay(1000);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка підключення до бази данних: {ex.Message}");
            await Task.Delay(1000);
        }

        // Ініціалізація юніту робочого процесу та сервісів
        IUnitOfWork uow = new UnitOfWork(dbContext);

        // Налаштування AutoMapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        // Ініціалізація конфігурації AutoMapper
        var mapper = config.CreateMapper();

        // Ініціалізація сервісів
        var roomService = new RoomService(uow, mapper);
        // Ініціалізація сервісу кімнат
        var bookingService = new BookingService(uow);

        // Ініціалізація сервісів
        var ui = new ConsoleUI(roomService, bookingService);

        // Інтерфейс користувача
        await ui.RunAsync();
    }
}