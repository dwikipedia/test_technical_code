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

        private List<SelectListItem> GetDurationOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "15", Text = "15 minutes" },
                new SelectListItem { Value = "30", Text = "30 minutes" },
                new SelectListItem { Value = "45", Text = "45 minutes" },
                new SelectListItem { Value = "60", Text = "1 hour" },
                new SelectListItem { Value = "90", Text = "1.5 hours" },
                new SelectListItem { Value = "120", Text = "2 hours" }
            };
        }

        public ActionResult Index()
        {
            var model = new BookViewModel
            {
                Time = "08.00am",
                DurationInMinutes = GetDurationOptions()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Book(BookViewModel vm)
        {
            vm.DurationInMinutes = GetDurationOptions();

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
                
                ModelState.AddModelError("", "Failed to book a meeting room");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error Occurred: " + ex.Message);
            }

            return View("Index", vm);
        }
    }
}