using AntiCafeConsoleApp.BusinessLogic.DTO;
using AntiCafeConsoleApp.DataAccess.Entity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCafeConsoleApp.BusinessLogic
{
    internal class MappingProfile : Profile
    {
        // Конфігурація мапінгу між сутностями та DTO
        public MappingProfile()
        {
            CreateMap<Room, RoomDto>();
            CreateMap<Booking, BookingDto>();
            CreateMap<Activity, ActivityDto>();

            // Якщо є DTO → Entity (двостороннє мапування)
            CreateMap<RoomDto, Room>();
            CreateMap<BookingDto, Booking>();
            CreateMap<ActivityDto, Activity>();
        }
    }
}
