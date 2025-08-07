using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMeetingRoom.Dto
{
    public class MeetingRoom
    {
        public string Name { get; set; }
        public List<TimeSlot> AvailableSlots { get; set; }
    }

    public class TimeSlot
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
