using AntiCafeConsoleApp.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AntiCafeConsoleApp.BusinessLogic.DTO;

namespace AntiCafeConsoleApp.BusinessLogic
{
    internal class RoomService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoomDto>> GetAvailableRooms(DateTime start, DateTime end)
        {
            var allRooms = await _uow.Rooms.GetAllAsync();
            var bookings = await _uow.Bookings.GetAllAsync();

            Console.WriteLine($"Start: {start}, Finish: {end}");

            var busyRoomIds = bookings
                .Where(b => start < b.EndTime && end > b.StartTime)
                .Select(b => b.RoomId)
                .Distinct();

            Console.WriteLine($"Bookings count: {busyRoomIds.Count()}");

            Console.WriteLine($"Busy room IDs: {string.Join(", ", busyRoomIds)}");

            var availableRooms = allRooms
                .Where(r => !busyRoomIds.Contains(r.Id));

            Console.WriteLine($"Available rooms count: {availableRooms.Count()}");
            Console.WriteLine($"Booking rooms count: {bookings.Count()}");

            return _mapper.Map<IEnumerable<RoomDto>>(availableRooms);
        }
    }
}
