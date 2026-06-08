using System;
using System.Collections.Generic;
using Task5.Models;

namespace Task5.Data
{
    public static class InMemoryStore
    {
        public static List<Room> Rooms { get; set; } = new List<Room>
        {
            new Room { Id = 1, Name = "Lab 101", BuildingCode = "A", Floor = 1, Capacity = 30, HasProjector = true, IsActive = true },
            new Room { Id = 2, Name = "Lab 102", BuildingCode = "A", Floor = 1, Capacity = 15, HasProjector = false, IsActive = true },
            new Room { Id = 3, Name = "Lecture Hall 1", BuildingCode = "B", Floor = 2, Capacity = 100, HasProjector = true, IsActive = true },
            new Room { Id = 4, Name = "Meeting Room 1", BuildingCode = "C", Floor = 1, Capacity = 8, HasProjector = true, IsActive = false },
            new Room { Id = 5, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true, IsActive = true }
        };

        public static List<Reservation> Reservations { get; set; } = new List<Reservation>
        {
            new Reservation { Id = 1, RoomId = 1, OrganizerName = "John Doe", Topic = "C# Basics", Date = new DateTime(2026, 5, 10), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(11, 0, 0), Status = "confirmed" },
            new Reservation { Id = 2, RoomId = 2, OrganizerName = "Jane Smith", Topic = "ASP.NET Core", Date = new DateTime(2026, 5, 10), StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(14, 0, 0), Status = "planned" },
            new Reservation { Id = 3, RoomId = 5, OrganizerName = "Anna Kowalska", Topic = "HTTP and REST Workshop", Date = new DateTime(2026, 5, 10), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 30, 0), Status = "confirmed" },
            new Reservation { Id = 4, RoomId = 3, OrganizerName = "Bob", Topic = "Architecture", Date = new DateTime(2026, 5, 11), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(16, 0, 0), Status = "confirmed" },
            new Reservation { Id = 5, RoomId = 1, OrganizerName = "Alice", Topic = "1-on-1 Consultations", Date = new DateTime(2026, 5, 12), StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(15, 0, 0), Status = "cancelled" }
        };
    }
}