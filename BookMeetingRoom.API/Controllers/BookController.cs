using BookMeetingRoom.API.DbContext;
using BookMeetingRoom.Data;
using BookMeetingRoom.Dto.Constant;
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var meetingRooms = MeetingRoomData.Rooms;
                TimeSpan proposedEndTime = TimeSpan.Parse(data.Time).Add(data.Duration);

                if (data.NumOfPeople == 1)
                {
                    return BadRequest(new ApiResponse<BookingData>
                    {
                        Message = "Rejected, why would you want to have a meeting by yourself?",
                        Data = new List<BookingData>()
                    });
                }

                var suggestions = new List<BookingData>();
                CheckRoomAvailability(meetingRooms, proposedEndTime, suggestions);

                var response = CheckSuggestions(suggestions);

                if (response != null)
                    return BadRequest(response);

                //Good to go! Let's insert to db!
                var book = new Book
                {
                    Name = data.Name,
                    Duration = data.Duration,
                    NumOfPeople = data.NumOfPeople,
                    Time = data.Time
                };

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBookById), new { id = data.Id }, book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error Occurred: " + ex.Message);
            }
        }

        private void CheckRoomAvailability(
            List<MeetingRoom> meetingRooms,
            TimeSpan proposedTime,
            List<BookingData> suggestions)
        {
            foreach (var i in meetingRooms)
            {
                var foundSlot = i.AvailableSlots
                    .FirstOrDefault(slot => proposedTime >= slot.StartTime && proposedTime <= slot.EndTime);

                if (foundSlot != null)
                {
                    var suggestion = new BookingData
                    {
                        Id = i.Id,
                        RoomName = i.Name,
                        StartTime = DateTime.Today.Add(foundSlot.StartTime).ToString("hh:mm tt"),
                        EndTime = DateTime.Today.Add(foundSlot.EndTime).ToString("hh:mm tt")
                    };

                    suggestions.Add(suggestion);
                }
            }
        }

        private ApiResponse<BookingData> CheckSuggestions(List<BookingData> suggestions)
        {
            var response = new ApiResponse<BookingData>();

            if (suggestions.Any())
            {
                var slots = suggestions
                    .Select(s => $"{s.EndTime} in {s.RoomName}")
                    .ToList();

                response = new ApiResponse<BookingData>
                {
                    Message = $"Rejected, available on {string.Join(" and ", slots)}",
                    Data = suggestions
                };
            }

            return response;
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
