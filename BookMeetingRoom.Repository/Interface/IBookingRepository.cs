using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMeetingRoom.Repository.Interface
{
    public interface IBookingRepository
    {
        Task<ApiResponse<List<BookingData>>> BookRoomAsync(BookDto dto);
    }
}
