using BookMeetingRoom.Dto;
using BookMeetingRoom.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookMeetingRoom.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly HttpClient _httpClient;

        public BookController()
        {
            _httpClient = new HttpClient();
            var baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult Index()
        {
            var model = new BookViewModel
            {
                Time = "08.00am",
                TimeOptions = GetTimeOptions(),
                DurationInMinutes = GetDurationOptions()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Book(BookViewModel vm)
        {
            vm.DurationInMinutes = GetDurationOptions();
            vm.TimeOptions = GetTimeOptions();

            if (!ModelState.IsValid)
            {
                return View("Index", vm);
            }

            try
            {
                var dto = new BookDto
                {
                    Id = vm.Id,
                    Name = vm.Name,
                    NumOfPeople = vm.NumOfPeople,
                    Time = vm.Time,
                    Duration = TimeSpan.FromMinutes(vm.Duration)

                };

                var jsonContent = JsonConvert.SerializeObject(dto);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("book", content);
                if (response.IsSuccessStatusCode)
                {
                    return View("Index", new BookViewModel());
                }
                else
                {
                    string message = await response.Content.ReadAsStringAsync();

                    ViewData["BookingMessage"] = message;

                    ModelState.AddModelError("", "Failed to book a meeting room");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error Occurred: " + ex.Message);
            }

            return View("Index", vm);
        }

        private List<SelectListItem> GetTimeOptions()
        {
            var times = new List<SelectListItem>();

            DateTime start = DateTime.Today.AddHours(7);      // 7:00 AM
            DateTime end = DateTime.Today.AddHours(16.5);     // 4:30 PM

            for (var time = start; time <= end; time = time.AddMinutes(15))
            {
                times.Add(new SelectListItem
                {
                    Value = time.ToString("HH:mm"),  // 24-hour format value
                    Text = time.ToString("h:mm tt")  // Display as 7:00 AM, 7:15 AM, etc.
                });
            }

            return times;
        }

        private List<SelectListItem> GetDurationOptions()
        {
            var list = new List<SelectListItem>();

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