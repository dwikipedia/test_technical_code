using BookMeetingRoom.Dto.Constant;
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
        public BookViewModel()
        {
            Init();
        }

        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Duration")]
        [Required]
        public int Duration { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Time")]
        [Required]
        public string Time { get; set; }

        [Display(Name = "Number of People")]
        [Required]
        [Range(2, 10, ErrorMessage = "Number of people must be between 2 and 10")]
        public int NumOfPeople { get; set; }

        public List<SelectListItem> DurationInMinutes { get; set; }
        public List<SelectListItem> TimeOptions { get; set; }

        private void Init()
        {
            TimeOptions = TimeHelper.SetTimeOptions();
            DurationInMinutes = TimeHelper.SetDurationOptions();
        }

    }
}