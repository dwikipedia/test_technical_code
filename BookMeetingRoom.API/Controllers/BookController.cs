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
                var book = new Book
                {
                    Name = data.Name,
                    Duration = data.Duration,
                    NumOfPeople = data.NumOfPeople,
                    Time = data.Time
                };

                //meeting room A = 5 ppl
                if (data.NumOfPeople > 0)
                {
                    if (data.NumOfPeople <= 5)
                    {

                    }

                    //meeting room B = 10 ppl
                    else if (data.NumOfPeople <= 10)
                    {
                    }
                }


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
