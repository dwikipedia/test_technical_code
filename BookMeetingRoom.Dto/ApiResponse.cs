using System.Collections.Generic;

namespace BookMeetingRoom.Data
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }

}
