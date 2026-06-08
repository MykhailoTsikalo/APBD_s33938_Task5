using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Task5.Data;
using Task5.Models;

namespace Task5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetReservations([FromQuery] DateTime? date, [FromQuery] string? status, [FromQuery] int? roomId)
        {
            var query = InMemoryStore.Reservations.AsQueryable();

            if (date.HasValue)
                query = query.Where(r => r.Date.Date == date.Value.Date);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

            if (roomId.HasValue)
                query = query.Where(r => r.RoomId == roomId.Value);

            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetReservationById(int id)
        {
            var reservation = InMemoryStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound($"Reservation with ID {id} not found.");

            return Ok(reservation);
        }

        [HttpPost]
        public IActionResult CreateReservation([FromBody] Reservation reservation)
        {
            var room = InMemoryStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
            if (room == null) return NotFound("The specified room does not exist.");
            if (!room.IsActive) return BadRequest("Cannot reserve an inactive room.");

            if (HasOverlap(reservation.RoomId, reservation.Date, reservation.StartTime, reservation.EndTime))
            {
                return Conflict("The reservation overlaps with an existing reservation for this room on the same day.");
            }

            reservation.Id = InMemoryStore.Reservations.Any() ? InMemoryStore.Reservations.Max(r => r.Id) + 1 : 1;
            InMemoryStore.Reservations.Add(reservation);

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
        {
            var reservation = InMemoryStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound($"Reservation with ID {id} not found.");

            var room = InMemoryStore.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);
            if (room == null) return NotFound("The specified room does not exist.");
            if (!room.IsActive) return BadRequest("Cannot reserve an inactive room.");

            
            if (HasOverlap(updatedReservation.RoomId, updatedReservation.Date, updatedReservation.StartTime, updatedReservation.EndTime, id))
            {
                return Conflict("The updated time overlaps with another reservation for this room.");
            }

            reservation.RoomId = updatedReservation.RoomId;
            reservation.OrganizerName = updatedReservation.OrganizerName;
            reservation.Topic = updatedReservation.Topic;
            reservation.Date = updatedReservation.Date;
            reservation.StartTime = updatedReservation.StartTime;
            reservation.EndTime = updatedReservation.EndTime;
            reservation.Status = updatedReservation.Status;

            return Ok(reservation);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            var reservation = InMemoryStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound($"Reservation with ID {id} not found.");

            InMemoryStore.Reservations.Remove(reservation);
            return NoContent();
        }

        
        private bool HasOverlap(int roomId, DateTime date, TimeSpan start, TimeSpan end, int? excludeReservationId = null)
        {
            return InMemoryStore.Reservations.Any(r =>
                r.RoomId == roomId &&
                r.Date.Date == date.Date &&
                r.Id != excludeReservationId &&
                r.Status.ToLower() != "cancelled" &&
                (start < r.EndTime && end > r.StartTime)
            );
        }
    }
}