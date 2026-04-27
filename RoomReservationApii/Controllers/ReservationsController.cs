using Microsoft.AspNetCore.Mvc;
using RoomReservationApii.Data;
using RoomReservationApii.Models;

namespace RoomReservationApii.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] DateOnly? date,
            [FromQuery] string? status,
            [FromQuery] int? roomId)
        {
            var reservations = DataStore.Reservations.AsEnumerable();

            if (date.HasValue)
                reservations = reservations.Where(r => r.Date == date.Value);

            if (!string.IsNullOrEmpty(status))
                reservations = reservations.Where(r => r.Status == status);

            if (roomId.HasValue)
                reservations = reservations.Where(r => r.RoomId == roomId.Value);

            return Ok(reservations.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                return NotFound($"Rezerwacja o id {id} nie istnieje.");

            return Ok(reservation);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Reservation reservation)
        {
            var room = DataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);

            if (room == null)
                return NotFound($"Sala o id {reservation.RoomId} nie istnieje.");

            if (!room.IsActive)
                return BadRequest("Nie można zarezerwować nieaktywnej sali.");

            bool hasConflict = DataStore.Reservations.Any(r =>
                r.RoomId == reservation.RoomId &&
                r.Date == reservation.Date &&
                r.StartTime < reservation.EndTime &&
                r.EndTime > reservation.StartTime);

            if (hasConflict)
                return Conflict("Rezerwacja koliduje z istniejącą rezerwacją.");

            reservation.Id = DataStore.Reservations.Max(r => r.Id) + 1;
            DataStore.Reservations.Add(reservation);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] Reservation reservation)
        {
            var existing = DataStore.Reservations.FirstOrDefault(r => r.Id == id);

            if (existing == null)
                return NotFound($"Rezerwacja o id {id} nie istnieje.");

            existing.RoomId = reservation.RoomId;
            existing.OrganizerName = reservation.OrganizerName;
            existing.Topic = reservation.Topic;
            existing.Date = reservation.Date;
            existing.StartTime = reservation.StartTime;
            existing.EndTime = reservation.EndTime;
            existing.Status = reservation.Status;

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                return NotFound($"Rezerwacja o id {id} nie istnieje.");

            DataStore.Reservations.Remove(reservation);
            return NoContent();
        }
    }
}