using BookMeetingRoom.API.DbContext;
using BookMeetingRoom.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BookMeetingRoom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookRoomDbContext _context;

        public BookController(BookRoomDbContext context) => _context = context;

        [HttpPost]
        public async Task<ActionResult<Book>> Book([FromBody] BookDto data)
        {
            var timeSlotsA = new List<TimeSlot>
            {
                new TimeSlot{StartTime=TimeSpan.FromHours(7), EndTime= TimeSpan.FromHours(9)},
                new TimeSlot{StartTime=TimeSpan.FromHours(9.45), EndTime= TimeSpan.FromHours(10.35)},
                new TimeSlot{StartTime=TimeSpan.FromHours(14), EndTime= TimeSpan.FromHours(15)},
            };

            var timeSlotsB = new List<TimeSlot>
            {
                new TimeSlot{StartTime=TimeSpan.FromHours(8), EndTime= TimeSpan.FromHours(10)},
                new TimeSlot{StartTime=TimeSpan.FromHours(10.45), EndTime= TimeSpan.FromHours(12)},
                new TimeSlot{StartTime=TimeSpan.FromHours(14), EndTime= TimeSpan.FromHours(15)},
            };

            var rooms = new List<MeetingRoom>
            {
                new() { Name = "Meeting Room A", AvailableSlots = timeSlotsA },
                new() { Name = "Meeting Room B", AvailableSlots = timeSlotsB }
            };

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //validation
                TimeSpan proposedTime = TimeSpan.Parse(data.Time).Add(data.Duration);

                string message = "";
                //string slotA = "", slotB = "";
                var suggestions = new List<BookingData>();

                if (data.NumOfPeople > 0)
                {
                    // Room A (max 5 ppl)
                    CheckRoomAvailability(timeSlotsA, 1, 5, data.NumOfPeople, proposedTime, suggestions);

                    // Room B (max 10 ppl)
                    CheckRoomAvailability(timeSlotsB, 2, 10, data.NumOfPeople, proposedTime, suggestions);
                }

                if (suggestions.Any())
                {
                    var roomMappings = new Dictionary<int, string>
                    {
                        { 1, "Room A" },
                        { 2, "Room B" }
                    };

                    var slots = roomMappings
                        .Select(m =>
                        {
                            var endTime = suggestions.FirstOrDefault(x => x.Id == m.Key)?.EndTime;
                            return endTime != null ? $"{endTime} in {m.Value}" : null;
                        })
                        .Where(s => s != null)
                        .ToList();

                    message = $"Rejected, available on {string.Join(" and ", slots)}";

                    var response = new ApiResponse<BookingData>
                    {
                        Message = message,
                        Data = suggestions
                    };

                    return BadRequest(response);
                }

                //Good to go! Let's insert to db!
                var book = new Book
                {
                    Name = data.Name,
                    Duration = data.Duration,
                    NumOfPeople = data.NumOfPeople,
                    Time = data.Time
                };

                await _context.Books.AddAsync(book);

                //insert to local db
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBookById), new { id = data.Id }, book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error Occurred: " + ex.Message);
            }
        }

        private void CheckRoomAvailability(
            List<TimeSlot> timeSlots,
            int roomId,
            int capacityLimit,
            int numOfPeople,
            TimeSpan proposedTime,
            List<BookingData> suggestions)
        {
            if (numOfPeople > capacityLimit)
                return;

            var foundSlot = timeSlots
                .FirstOrDefault(slot => proposedTime >= slot.StartTime && proposedTime <= slot.EndTime);

            if (foundSlot != null)
            {
                var suggestion = new BookingData
                {
                    Id = roomId,
                    StartTime = DateTime.Today.Add(foundSlot.StartTime).ToString("hh:mm tt"),
                    EndTime = DateTime.Today.Add(foundSlot.EndTime).ToString("hh:mm tt")
                };

                suggestions.Add(suggestion);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            return Ok(book);
        }
    }
}
