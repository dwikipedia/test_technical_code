using System.Collections.Generic;

namespace BookMeetingRoom.Dto
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }

}
