using System;
using System.Collections.Generic;

namespace BookMeetingRoom.Data
{
    public class MeetingRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TimeSlot> AvailableSlots { get; set; }
    }

    public class TimeSlot
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
