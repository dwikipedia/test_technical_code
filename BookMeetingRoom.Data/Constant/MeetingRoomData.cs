using BookMeetingRoom.Data;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BookMeetingRoom.Data.Constant
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

    public static class TimeHelper
    {
        public static List<SelectListItem> SetTimeOptions(
            int startHour = 7,
            double endHour = 16.5,
            int intervalMinutes = 15)
        {
            var times = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Select Time --" }
            };

            DateTime start = DateTime.Today.AddHours(startHour);
            DateTime end = DateTime.Today.AddHours(endHour);

            for (var time = start; time <= end; time = time.AddMinutes(intervalMinutes))
            {
                times.Add(new SelectListItem
                {
                    Value = time.ToString("HH:mm"),
                    Text = time.ToString("h:mm tt")
                });
            }

            return times;
        }

        public static List<SelectListItem> SetDurationOptions()
        {
            var list = new List<SelectListItem>{
                 new SelectListItem { Value = "", Text = "-- Select Duration --" }
            };

            for (int minutes = 15; minutes <= 120; minutes += 15)
            {
                string text;

                if (minutes < 60)
                {
                    text = $"{minutes} minutes";
                }
                else
                {
                    int hours = minutes / 60;
                    int mins = minutes % 60;

                    if (mins == 0)
                        text = $"{hours} hour{(hours > 1 ? "s" : "")}";
                    else if (mins == 15)
                        text = $"{hours} hour {mins} min";
                    else if (mins == 30)
                        text = $"{hours}.5 hours";
                    else
                        text = $"{hours} hour {mins} min";
                }

                list.Add(new SelectListItem
                {
                    Value = minutes.ToString(),
                    Text = text
                });
            }

            return list;
        }
    }

}
