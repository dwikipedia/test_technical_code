using BookMeetingRoom.Service.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookMeetingRoom.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResult> BookRoomAsync(BookViewModel vm)
        {
            var dto = new BookDto
            {
                Id = vm.Id,
                Name = vm.Name,
                NumOfPeople = vm.NumOfPeople,
                Time = vm.Time,
                Duration = TimeSpan.FromMinutes(vm.Duration)
            };

            var apiResponse = await _bookingRepository.BookRoomAsync(dto);

            if (apiResponse.Success)
            {
                return new BookingResult
                {
                    IsSuccess = true
                };
            }
            else
            {
                return new BookingResult
                {
                    IsSuccess = false,
                    Message = apiResponse.Message,
                    Suggestions = apiResponse.Data
                        ?.Select(s => new SuggestionViewModel
                        {
                            Id = s.Id,
                            RoomName = s.RoomName,
                            StartTime = s.StartTime,
                            EndTime = s.EndTime
                        })
                        .ToList()
                };
            }
        }
    }

}
