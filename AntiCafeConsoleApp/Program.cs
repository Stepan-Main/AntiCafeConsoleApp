using AntiCafeConsoleApp.BusinessLogic;
using AntiCafeConsoleApp.DataAccess;
using AntiCafeConsoleApp.DataAccess.UnitOfWork;
using AntiCafeConsoleApp.DataAccess.Entity;
using AutoMapper;

class Program
{
    static async Task Main(string[] args)
    {
        var dbContext = new AppDbContext();
        IUnitOfWork uow = new UnitOfWork(dbContext);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        var mapper = config.CreateMapper();

        var roomService = new RoomService(uow, mapper);

        Console.WriteLine("Введіть час початку (yyyy-MM-dd HH:mm):");
        var start = DateTime.Parse(Console.ReadLine());

        Console.WriteLine("Введіть час закінчення (yyyy-MM-dd HH:mm):");
        var end = DateTime.Parse(Console.ReadLine());

        var rooms = await roomService.GetAvailableRooms(start, end);
        foreach (var room in rooms)
            Console.WriteLine($"Доступна зала: {room.Name}");
    }
}