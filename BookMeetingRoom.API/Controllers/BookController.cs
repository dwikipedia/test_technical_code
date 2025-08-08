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
                string slotA = "", slotB = "";

                if (data.NumOfPeople > 0)
                {
                    if (data.NumOfPeople <= 5)
                    {
                        var foundSlot = timeSlotsA
                            .FirstOrDefault(slot => proposedTime >= slot.StartTime && proposedTime <= slot.EndTime);

                        //rejected, suggest available time slot
                        if (foundSlot != null)
                        {
                            slotA = DateTime.Today.Add(foundSlot.EndTime).ToString("hh:mm tt");
                        }
                    }

                    //meeting room B = 10 ppl
                    if (data.NumOfPeople <= 10)
                    {
                        var foundSlot = timeSlotsB
                            .FirstOrDefault(slot => proposedTime >= slot.StartTime && proposedTime <= slot.EndTime);

                        if (foundSlot != null)
                        {
                            slotB = DateTime.Today.Add(foundSlot.EndTime).ToString("hh:mm tt");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(slotA) || !string.IsNullOrEmpty(slotB))
                {
                    var slots = new List<string>();

                    if (!string.IsNullOrEmpty(slotA)) slots.Add($"{slotA} in Room A");
                    if (!string.IsNullOrEmpty(slotB)) slots.Add($"{slotB} in Room B");

                    message = $"Rejected, available on {string.Join(" and ", slots)}";

                    return BadRequest(message);
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            return Ok(book);
        }
    }
}
