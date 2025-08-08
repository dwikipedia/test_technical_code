using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMeetingRoom.Data
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumOfPeople { get; set; }
        public string Time { get; set; }
        public TimeSpan Duration { get; set; }

    }
}
