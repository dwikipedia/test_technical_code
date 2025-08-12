using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMeetingRoom.Data
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public string Time { get; set; }
        public int NumOfPeople { get; set; }
    }
}
