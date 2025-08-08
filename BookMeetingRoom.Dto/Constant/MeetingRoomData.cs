using BookMeetingRoom.Data;
using System;
using System.Collections.Generic;

namespace BookMeetingRoom.Dto.Constant
{
    public static class MeetingRoomData
    {
        public static readonly List<MeetingRoom> Rooms = new List<MeetingRoom>
        {
            new MeetingRoom
            {
                Id = 1,
                Name = "Meeting Room A",
                AvailableSlots = new List<TimeSlot>
                {
                    new TimeSlot{StartTime = TimeSpan.FromHours(7), EndTime = TimeSpan.FromHours(9)},
                    new TimeSlot{StartTime = TimeSpan.FromHours(9.45), EndTime = TimeSpan.FromHours(10.35)},
                    new TimeSlot{StartTime = TimeSpan.FromHours(14), EndTime = TimeSpan.FromHours(15)},
                }
            },
            new MeetingRoom
            {
                Id= 2,
                Name = "Meeting Room B",
                AvailableSlots = new List<TimeSlot>
                {
                    new TimeSlot{StartTime = TimeSpan.FromHours(8), EndTime = TimeSpan.FromHours(10)},
                    new TimeSlot{StartTime = TimeSpan.FromHours(10.45), EndTime = TimeSpan.FromHours(12)},
                    new TimeSlot{StartTime = TimeSpan.FromHours(14), EndTime = TimeSpan.FromHours(15)},
                }
            }
        };
    }
}
