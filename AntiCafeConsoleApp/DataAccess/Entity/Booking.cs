using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiCafeConsoleApp.DataAccess.Entity
{
    // Ентитет для зберігання інформації про бронювання
    internal class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int EventPackageId { get; set; }
        public EventPackage EventPackage { get; set; } = null!;
        public Room Room { get; set; } = null!;
    }
}
