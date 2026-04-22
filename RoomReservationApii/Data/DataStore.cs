using RoomReservationApii.Models;

namespace RoomReservationApii.Data
{
    public static class DataStore
    {
        public static List<Room> Rooms { get; } = new List<Room>
        {
            new Room { Id = 1, Name = "Lab 101", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true, IsActive = true },
            new Room { Id = 2, Name = "Lab 102", BuildingCode = "A", Floor = 1, Capacity = 15, HasProjector = false, IsActive = true },
            new Room { Id = 3, Name = "Lab 201", BuildingCode = "B", Floor = 2, Capacity = 30, HasProjector = true, IsActive = true },
            new Room { Id = 4, Name = "Lab 202", BuildingCode = "B", Floor = 2, Capacity = 25, HasProjector = true, IsActive = false },
            new Room { Id = 5, Name = "Lab 301", BuildingCode = "C", Floor = 3, Capacity = 10, HasProjector = false, IsActive = true }
        };

        public static List<Reservation> Reservations { get; } = new List<Reservation>
        {
            new Reservation { Id = 1, RoomId = 1, OrganizerName = "Jan  Brzechwa", Topic = "Warsztaty C#", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 0), Status = "confirmed" },
            new Reservation { Id = 2, RoomId = 1, OrganizerName = "Adam Mickiewicz", Topic = "Szkolenie SQL", Date = new DateOnly(2026, 5, 11), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0), Status = "planned" },
            new Reservation { Id = 3, RoomId = 2, OrganizerName = "Julian Tuwim", Topic = "Warsztaty HTTP", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(15, 0), Status = "confirmed" },
            new Reservation { Id = 4, RoomId = 3, OrganizerName = "Zbigniew Herbert", Topic = "Szkolenie Git", Date = new DateOnly(2026, 5, 12), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 0), Status = "planned" },
            new Reservation { Id = 5, RoomId = 2, OrganizerName = "Robert Lewandowski", Topic = "Warsztaty REST", Date = new DateOnly(2026, 5, 13), StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(16, 0), Status = "cancelled" }
        };
    }
}