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
        public MappingProfile()
        {
            CreateMap<Room, RoomDto>();
            CreateMap<Booking, BookingDto>();
            CreateMap<Activity, ActivityDto>();

            CreateMap<RoomDto, Room>();
            CreateMap<BookingDto, Booking>();
            CreateMap<ActivityDto, Activity>();
        }
    }
}
