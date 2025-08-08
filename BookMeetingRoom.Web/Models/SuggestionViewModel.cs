using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Web.Models
{
    public class SuggestionViewModel
    {
        public string RoomName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}