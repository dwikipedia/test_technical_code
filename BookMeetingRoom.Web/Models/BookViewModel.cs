using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookMeetingRoom.Web.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Duration")]
        public int Duration { get; set; }
        public List<SelectListItem> DurationInMinutes { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Time")]
        //public DateTime Time { get; set; }
        public string Time { get; set; }

        [Display(Name = "Number of People")]
        public int NumOfPeople { get; set; }
    }
}