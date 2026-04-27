using Microsoft.AspNetCore.Mvc;
using RoomReservationApii.Data;
using RoomReservationApii.Models;

namespace RoomReservationApii.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? minCapacity,
            [FromQuery] bool? hasProjector,
            [FromQuery] bool? activeOnly)
        {
            var rooms = DataStore.Rooms.AsEnumerable();

            if (minCapacity.HasValue)
                rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);

            if (hasProjector.HasValue)
                rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);

            if (activeOnly.HasValue && activeOnly.Value)
                rooms = rooms.Where(r => r.IsActive);

            return Ok(rooms.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);

            if (room == null)
                return NotFound($"Sala o id {id} nie istnieje.");

            return Ok(room);
        }

        [HttpGet("building/{buildingCode}")]
        public IActionResult GetByBuilding([FromRoute] string buildingCode)
        {
            var rooms = DataStore.Rooms.Where(r => r.BuildingCode == buildingCode).ToList();

            if (!rooms.Any())
                return NotFound($"Brak sal w budynku {buildingCode}.");

            return Ok(rooms);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Room room)
        {
            room.Id = DataStore.Rooms.Max(r => r.Id) + 1;
            DataStore.Rooms.Add(room);
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }
 
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] Room room)
        {
            var existing = DataStore.Rooms.FirstOrDefault(r => r.Id == id);

            if (existing == null)
                return NotFound($"Sala o id {id} nie istnieje.");

            existing.Name = room.Name;
            existing.BuildingCode = room.BuildingCode;
            existing.Floor = room.Floor;
            existing.Capacity = room.Capacity;
            existing.HasProjector = room.HasProjector;
            existing.IsActive = room.IsActive;

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);

            if (room == null)
                return NotFound($"Sala o id {id} nie istnieje.");

            bool hasFutureReservations = DataStore.Reservations
                .Any(res => res.RoomId == id && res.Date >= DateOnly.FromDateTime(DateTime.Today));

            if (hasFutureReservations)
                return Conflict("Nie można usunąć sali która ma przyszłe rezerwacje.");

            DataStore.Rooms.Remove(room);
            return NoContent();
        }
    }
}