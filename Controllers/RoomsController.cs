using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Task5.Data;
using Task5.Models;

namespace Task5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
        {
            var query = InMemoryStore.Rooms.AsQueryable();

            if (minCapacity.HasValue)
                query = query.Where(r => r.Capacity >= minCapacity.Value);

            if (hasProjector.HasValue)
                query = query.Where(r => r.HasProjector == hasProjector.Value);

            if (activeOnly.HasValue && activeOnly.Value)
                query = query.Where(r => r.IsActive);

            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetRoomById(int id)
        {
            var room = InMemoryStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound($"Room with ID {id} not found.");

            return Ok(room);
        }

        [HttpGet("building/{buildingCode}")]
        public IActionResult GetRoomsByBuilding(string buildingCode)
        {
            var rooms = InMemoryStore.Rooms
                .Where(r => r.BuildingCode.Equals(buildingCode, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(rooms);
        }

        [HttpPost]
        public IActionResult CreateRoom([FromBody] Room room)
        {
            room.Id = InMemoryStore.Rooms.Any() ? InMemoryStore.Rooms.Max(r => r.Id) + 1 : 1;
            InMemoryStore.Rooms.Add(room);

            return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            var room = InMemoryStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound($"Room with ID {id} not found.");

            room.Name = updatedRoom.Name;
            room.BuildingCode = updatedRoom.BuildingCode;
            room.Floor = updatedRoom.Floor;
            room.Capacity = updatedRoom.Capacity;
            room.HasProjector = updatedRoom.HasProjector;
            room.IsActive = updatedRoom.IsActive;

            return Ok(room);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            var room = InMemoryStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound($"Room with ID {id} not found.");

            bool hasReservations = InMemoryStore.Reservations.Any(res => res.RoomId == id);
            if (hasReservations)
            {
                return Conflict($"Cannot delete room {id} because it has existing reservations.");
            }

            InMemoryStore.Rooms.Remove(room);
            return NoContent();
        }
    }
}