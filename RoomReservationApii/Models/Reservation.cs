using System.ComponentModel.DataAnnotations;

namespace RoomReservationApii.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        [Required]
        public string OrganizerName { get; set; } = string.Empty;

        [Required]
        public string Topic { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string Status { get; set; } = "planned";
    }
}